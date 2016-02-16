﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Registers;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Registers;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.SystemConfig.Registers
{
    /// <summary>
    /// Предоставляет методы для работы с регистрами.
    /// </summary>
    internal sealed class RegisterApi : IRegisterApi
    {
        // TODO: Нужен глубокий рефакторинг и тестирование.

        public RegisterApi(IAppEnvironment appEnvironment, IMetadataApi metadataApi, IDocumentApi documentApi, Func<string, IDocumentStorage> documentStorageFactory)
        {
            _appEnvironment = appEnvironment;
            _metadataApi = metadataApi;
            _documentApi = documentApi;
            _documentStorageFactory = documentStorageFactory;
        }

        private readonly IAppEnvironment _appEnvironment;
        private readonly IMetadataApi _metadataApi;
        private readonly IDocumentApi _documentApi;
        private readonly Func<string, IDocumentStorage> _documentStorageFactory;

        /// <summary>
        /// Создает (но не сохраняет) запись регистра.
        /// </summary>
        public dynamic CreateEntry(string registerName, string documentId, DateTime? documentDate, dynamic document, bool isInfoRegister)
        {
            if (string.IsNullOrEmpty(registerName))
            {
                throw new ArgumentNullException(nameof(registerName));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (documentId == null)
            {
                documentId = document.Id;
            }

            if (string.IsNullOrEmpty(documentId))
            {
                throw new ArgumentNullException(nameof(documentId));
            }

            dynamic registerEntry = new DynamicWrapper();
            registerEntry[RegisterConstants.RegistrarProperty] = document.Id;
            registerEntry[RegisterConstants.RegistrarTypeProperty] = documentId;
            registerEntry[RegisterConstants.EntryTypeProperty] = RegisterEntryType.Other;

            if (documentDate == null)
            {
                // Дата документа явно не задана, используем дату из содержимого переданного документа
                dynamic documentEvents = _metadataApi.GetDocumentEvents(documentId);
                string dateFieldName = (documentEvents.RegisterPoint != null) ? documentEvents.RegisterPoint.DocumentDateProperty : null;

                if (!string.IsNullOrEmpty(dateFieldName))
                {
                    documentDate = document[dateFieldName];
                }
            }

            var registerMetadata = _metadataApi.GetRegister(registerName);

            // Признак того, что необходимо создать запись для регистра сведений
            if (isInfoRegister && registerMetadata != null)
            {
                documentDate = RegisterPeriod.AdjustDateToPeriod(documentDate.Value, registerMetadata.Period);
            }

            registerEntry[RegisterConstants.DocumentDateProperty] = documentDate;

            return registerEntry;
        }

        /// <summary>
        /// Выполняет проведение данных документа в регистр.
        /// </summary>
        public void PostEntries(string registerName, IEnumerable<object> registerEntries)
        {
            if (string.IsNullOrEmpty(registerName))
            {
                throw new ArgumentNullException(nameof(registerName));
            }

            if (registerEntries == null)
            {
                throw new ArgumentNullException(nameof(registerEntries));
            }

            var registerObject = _metadataApi.GetRegister(registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName} not found'");
            }

            var dimensionNames = new List<string>();

            foreach (var property in registerObject.Properties)
            {
                if (property.Type == RegisterPropertyType.Dimension)
                {
                    dimensionNames.Add(property.Property);
                }
            }

            foreach (dynamic registerEntry in registerEntries)
            {
                // Id генерируется по следующему алгоритму:
                // формируется уникальный ключ записи по всем полям-измерениям и по полю даты,
                // далее из уникального ключа рассчитывается Guid записи

                if (registerEntry[RegisterConstants.DocumentDateProperty] == null)
                {
                    throw new InvalidOperationException("Property 'DocumentDate' should be in the registry entry");
                }

                string uniqueKey = registerEntry[RegisterConstants.DocumentDateProperty].ToString();

                foreach (var dimensionName in dimensionNames)
                {
                    if (registerEntry[dimensionName] != null)
                    {
                        uniqueKey += registerEntry[dimensionName].ToString();
                    }
                }

                using (var md5 = MD5.Create())
                {
                    var hash = md5.ComputeHash(Encoding.Default.GetBytes(uniqueKey));
                    registerEntry.Id = new Guid(hash).ToString();
                }
            }

            _documentApi.SetDocuments(RegisterConstants.RegisterNamePrefix + registerName, registerEntries);
        }

        /// <summary>
        /// Выполняет перепроведение документов до указанной даты.
        /// </summary>
        public void RecarryingEntries(string registerName, DateTime aggregationDate, bool deteleExistingRegisterEntries = true)
        {
            // TODO: Механизм перепроведения нуждается в переработке!

            // Один документ может создать две записи в одном регистре в ходе проведения, 
            // однако перепровести документ нужно только один раз. Для этого будем хранить
            // список идентификаторов уже перепроведенных документов
            var recarriedDocuments = new List<string>();

            var pageNumber = 0;

            while (true)
            {
                // Получаем записи из регистра постранично
                Action<FilterBuilder> filter = f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThanOrEquals(aggregationDate));

                var registerEntries = _documentApi.GetDocument(RegisterConstants.RegisterNamePrefix + registerName,
                    filter,
                    pageNumber++, 1000).ToArray();

                if (registerEntries.Length == 0)
                {
                    break;
                }

                // Перепроводить документы нужно все сразу после удаления соответствующих
                // записей из регистра. Поэтому сначала сохраняем пары
                // <Тип документа - содержимое документа> чтобы далее выполнить SetDocument для каждого элемента
                var documentsToRecarry = new List<Tuple<string, dynamic>>();

                foreach (var registerEntry in registerEntries)
                {
                    // Получаем документ-регистратор
                    string registrarId = registerEntry.Registrar;
                    string registrarType = registerEntry.RegistrarType;

                    var documentRegistrar = _documentApi.GetDocumentById(registrarType, registrarId);

                    if (deteleExistingRegisterEntries)
                    {
                        // Удаляем запись из регистра
                        _documentApi.DeleteDocument(RegisterConstants.RegisterNamePrefix + registerName, registerEntry.Id);
                    }

                    if (documentRegistrar != null && !recarriedDocuments.Contains(registrarId))
                    {
                        documentsToRecarry.Add(new Tuple<string, dynamic>(registrarType, documentRegistrar));
                        recarriedDocuments.Add(registrarId);
                    }
                }

                foreach (var document in documentsToRecarry)
                {
                    // Перепроводка документа
                    _documentApi.SetDocument(document.Item1, document.Item2);
                }
            }

            // Удаляем значения из таблицы итогов
            Action<FilterBuilder> action = f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThanOrEquals(aggregationDate));

            var registerTotalEntries = _documentApi.GetDocument(RegisterConstants.RegisterTotalNamePrefix + registerName,
                action,
                0, 1000);

            foreach (var registerEntry in registerTotalEntries)
            {
                _documentApi.DeleteDocument(RegisterConstants.RegisterTotalNamePrefix + registerName, registerEntry.Id);
            }
        }

        /// <summary>
        /// Рассчитывает итоги для регистров накопления на текущую дату.
        /// </summary>
        public void RecalculateTotals()
        {
            var registerName = _appEnvironment.Name + RegisterConstants.RegistersCommonInfo;

            var registersInfo = _documentApi.GetDocument(registerName, null, 0, 1000);

            var tempDate = DateTime.Now;

            var calculationDate = new DateTime(
                tempDate.Year,
                tempDate.Month,
                tempDate.Day,
                tempDate.Hour,
                tempDate.Minute,
                tempDate.Second);

            foreach (var registerInfo in registersInfo)
            {
                dynamic registerId = registerInfo.Id;

                var aggregatedData = GetValuesByDate(registerId, calculationDate);

                foreach (var item in aggregatedData)
                {
                    item.Id = Guid.NewGuid().ToString();
                    item[RegisterConstants.DocumentDateProperty] = calculationDate;

                    _documentApi.SetDocument(RegisterConstants.RegisterTotalNamePrefix + registerId, item);
                }
            }
        }

        /// <summary>
        /// Удаляет запись регистра.
        /// </summary>
        public void DeleteEntry(string registerName, string registar)
        {
            if (string.IsNullOrEmpty(registerName))
            {
                throw new ArgumentNullException(nameof(registerName));
            }

            if (string.IsNullOrEmpty(registar))
            {
                throw new ArgumentNullException(nameof(registar));
            }

            // Находим все записи в регистре, соответствующие регистратору
            var registerEntries = GetEntries(registerName,
                new[] { new FilterCriteria(RegisterConstants.RegistrarProperty, registar, CriteriaType.IsEquals) },
                0,
                1000);

            var earliestDocumentDate = DateTime.MaxValue;

            foreach (dynamic registerEntry in registerEntries)
            {
                _documentApi.DeleteDocument(RegisterConstants.RegisterNamePrefix + registerName, registerEntry.Id);

                var documentDate = registerEntry[RegisterConstants.DocumentDateProperty];

                if (documentDate < earliestDocumentDate)
                {
                    earliestDocumentDate = documentDate;
                }
            }

            // Необходимо удалить все записи регистра после earliestDocumentDate
            var notActualRegisterEntries = GetEntries(registerName,
                new[] { new FilterCriteria(RegisterConstants.DocumentDateProperty, earliestDocumentDate, CriteriaType.IsMoreThanOrEquals) },
                0,
                1000);

            foreach (dynamic registerEntry in notActualRegisterEntries)
            {
                _documentApi.DeleteDocument(RegisterConstants.RegisterNamePrefix + registerName, registerEntry.Id);
            }
        }

        /// <summary>
        /// Возвращает записи регистра.
        /// </summary>
        public IEnumerable<object> GetEntries(string registerName, IEnumerable<FilterCriteria> filter, int pageNumber, int pageSize)
        {
            return _documentApi.GetDocuments(RegisterConstants.RegisterNamePrefix + registerName,
                filter,
                pageNumber,
                pageSize);
        }

        /// <summary>
        /// Возвращает записи регистра.
        /// </summary>
        public IEnumerable<object> GetValuesByDate(string registerName, DateTime aggregationDate, IEnumerable<FilterCriteria> filter = null, IEnumerable<string> dimensionsProperties = null, IEnumerable<string> valueProperties = null, IEnumerable<AggregationType> aggregationTypes = null)
        {
            var registerObject = _metadataApi.GetRegister(registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName} not found'");
            }

            // Сначала необходимо извлечь значения из регистра итогов
            var closestDate = GetClosestDateTimeOfTotalCalculation(registerName, aggregationDate);

            var filetrBuilder = new FilterBuilder();
            filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThanOrEquals(aggregationDate));

            IEnumerable<dynamic> aggregatedTotals = null;

            if (closestDate != null)
            {
                aggregatedTotals = _documentApi.GetDocument(RegisterConstants.RegisterTotalNamePrefix + registerName,
                    f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsEquals(closestDate.Value)), 0, 1000);

                filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsMoreThan(closestDate.Value));
            }

            if (dimensionsProperties == null)
            {
                dimensionsProperties = AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject);
            }

            valueProperties = valueProperties ?? AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var valuePropertiesCount = valueProperties.Count();

            var resultFilter = new List<FilterCriteria>();

            if (filter != null)
            {
                resultFilter.AddRange(filter);
            }

            resultFilter.AddRange(filetrBuilder.CriteriaList);

            if (aggregationTypes == null)
            {
                // По умолчанию считаем сумму значений
                aggregationTypes = AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount);
            }

            IEnumerable<DynamicWrapper> aggregationResult = GetAggregationDocumentResult(
                RegisterConstants.RegisterNamePrefix + registerName,
                resultFilter,
                dimensionsProperties,
                valueProperties, 
                aggregationTypes);

            if (aggregatedTotals != null)
            {
                var dimensionNames = dimensionsProperties.ToArray();
                return AggregationUtils.MergeAggregaionResults(dimensionNames, valueProperties, aggregationResult, aggregatedTotals);
            }

            return aggregationResult;
        }

        /// <summary>
        /// Возвращает значения ресурсов в указанном диапазоне дат для регистра.
        /// </summary>
        public IEnumerable<object> GetValuesBetweenDates(string registerName, DateTime beginDate, DateTime endDate, IEnumerable<FilterCriteria> filter = null, IEnumerable<string> dimensionsProperties = null, IEnumerable<string> valueProperties = null, IEnumerable<AggregationType> aggregationTypes = null)
        {
            var registerObject = _metadataApi.GetRegister(registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName} not found'");
            }

            if (dimensionsProperties == null)
            {
                dimensionsProperties = AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject);
            }

            valueProperties = valueProperties ?? AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var valuePropertiesCount = valueProperties.Count();

            var resultFilter = new List<FilterCriteria>();

            if (filter != null)
            {
                resultFilter.AddRange(filter);
            }

            resultFilter.AddRange(FilterBuilder.DateRangeCondition(RegisterConstants.DocumentDateProperty, beginDate, endDate));

            if (aggregationTypes == null)
            {
                // По умолчанию считаем сумму значений
                aggregationTypes = AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount);
            }

            IEnumerable<DynamicWrapper> aggregationResult = GetAggregationDocumentResult(
                RegisterConstants.RegisterNamePrefix + registerName,
                resultFilter,
                dimensionsProperties,
                valueProperties, 
                aggregationTypes);

            return aggregationResult;
        }

        /// <summary>
        /// Возвращает значения ресурсов в указанном диапазоне дат c разбиением на периоды.
        /// </summary>
        public IEnumerable<object> GetValuesByPeriods(string registerName, DateTime beginDate, DateTime endDate, string interval, IEnumerable<FilterCriteria> filter, IEnumerable<string> dimensionsProperties = null, IEnumerable<string> valueProperties = null)
        {
            var registerObject = _metadataApi.GetRegister(registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName}' not found.");
            }

            // TODO: Данный метод предполагает группировку по части даты (году, месяцу, дню и т.п.)

            // При переходе на MongoDB эта функциональность была временно "потеряна", но ее легко восстановить.
            // В метод GetAggregationDocumentResult() нужно передавать не просто список измерений, а список ключей.
            // Одни ключи будут соответствовать именам полей, другие будут представлены выражениями (например, 
            // выражением выборки из даты года, месяца, дня и т.п.). Исходная реализация предполагала, что в 
            // качестве интервала могут быть указаны следующие значения: year, quarter, month, week, day, hour,
            // minute, second (Note: лучше передавать это значение в виде Enum). Функциональность не была 
            // восстановлена, так как на момент перехода она не использовалась, во-вторых, механизм регистров
            // и код, который его реализует, требует глубокой переработки.

            if (!CheckInterval(interval))
            {
                throw new ArgumentException($"Specified interval '{interval}' is invalid. Supported intervals: year, quarter, month, week, day, hour, minute, second.", nameof(interval));
            }

            if (dimensionsProperties == null)
            {
                dimensionsProperties = AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject);
            }

            dimensionsProperties = dimensionsProperties.Union(new[] { RegisterConstants.DocumentDateProperty });

            valueProperties = valueProperties ?? AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var valuePropertiesCount = valueProperties.Count();

            var resultFilter = new List<FilterCriteria>();

            if (filter != null)
            {
                resultFilter.AddRange(filter);
            }

            resultFilter.AddRange(FilterBuilder.DateRangeCondition(RegisterConstants.DocumentDateProperty, beginDate, endDate));

            IEnumerable<DynamicWrapper> aggregationResult = GetAggregationDocumentResult(
                RegisterConstants.RegisterNamePrefix + registerName,
                resultFilter,
                dimensionsProperties,
                valueProperties, 
                AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount));

            return aggregationResult;
        }

        /// <summary>
        /// Получение значений ресурсов по документу-регистратору.
        /// </summary>
        public IEnumerable<object> GetValuesByRegistrar(string registerName, string registrar, IEnumerable<string> dimensionsProperties = null, IEnumerable<string> valueProperties = null)
        {
            var registerObject = _metadataApi.GetRegister(registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName}' not found.");
            }

            if (dimensionsProperties == null)
            {
                dimensionsProperties = AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject);
            }

            valueProperties = valueProperties ?? AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var valuePropertiesCount = valueProperties.Count();

            var filetrBuilder = new FilterBuilder();
            filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.RegistrarProperty).IsEquals(registrar));

            IEnumerable<DynamicWrapper> aggregationResult = GetAggregationDocumentResult(
                RegisterConstants.RegisterNamePrefix + registerName,
                filetrBuilder.CriteriaList,
                dimensionsProperties,
                valueProperties, 
                AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount));

            return aggregationResult;
        }

        /// <summary>
        /// Получение значений ресурсов по типу документа-регистратора.
        /// </summary>
        public IEnumerable<object> GetValuesByRegistrarType(string registerName, string registrar, IEnumerable<string> dimensionsProperties = null, IEnumerable<string> valueProperties = null)
        {
            var registerObject = _metadataApi.GetRegister(registerName);

            if (registerObject == null)
            {
                throw new ArgumentException($"Register '{registerName}' not found.");
            }

            if (dimensionsProperties == null)
            {
                dimensionsProperties = AggregationUtils.BuildDimensionsFromRegisterMetadata(registerObject);
            }

            valueProperties = valueProperties ?? AggregationUtils.BuildValuePropertyFromRegisterMetadata(registerObject);

            var valuePropertiesCount = valueProperties.Count();

            var filetrBuilder = new FilterBuilder();
            filetrBuilder.AddCriteria(c => c.Property(RegisterConstants.RegistrarTypeProperty).IsEquals(registrar));

            IEnumerable<DynamicWrapper> aggregationResult = GetAggregationDocumentResult(
                RegisterConstants.RegisterNamePrefix + registerName,
                filetrBuilder.CriteriaList,
                dimensionsProperties,
                valueProperties,
                AggregationUtils.BuildAggregationType(AggregationType.Sum, valuePropertiesCount));

            return aggregationResult;
        }

        /// <summary>
        /// Получение значений из таблицы итогов на дату, ближайшую к заданной
        /// </summary>
        public IEnumerable<object> GetTotals(string registerName, DateTime aggregationDate)
        {
            var closestDate = GetClosestDateTimeOfTotalCalculation(registerName, aggregationDate);

            if (closestDate != null)
            {
                Action<FilterBuilder> action = f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsEquals(aggregationDate));

                return _documentApi.GetDocument(RegisterConstants.RegisterTotalNamePrefix + registerName, action, 0, 1000);
            }

            return Enumerable.Empty<dynamic>();
        }

        /// <summary>
        /// Возвращает дату последнего подсчета итогов для регистра накоплений, ближайшей к заданной.
        /// </summary>
        public DateTime? GetClosestDateTimeOfTotalCalculation(string registerName, DateTime aggregationDate)
        {
            // В таблице итогов нужно найти итог, ближайший к aggregationDate
            var dateToReturn = new DateTime();

            var min = long.MaxValue;
            var isDateFound = false;

            var page = 0;

            while (true)
            {
                // Постранично считываем данные и таблицы итогов и ищем итоги с датой, ближайшей к заданной
                Action<FilterBuilder> filter = f => f.AddCriteria(c => c.Property(RegisterConstants.DocumentDateProperty).IsLessThan(aggregationDate));

                var totals = _documentApi.GetDocument(RegisterConstants.RegisterTotalNamePrefix + registerName, filter, page++, 1000).ToArray();

                if (totals.Length == 0)
                {
                    break;
                }

                foreach (var docWithTotals in totals)
                {
                    if (docWithTotals.DocumentDate != null)
                    {
                        if (Math.Abs(aggregationDate.Ticks - docWithTotals.DocumentDate.Ticks) < min)
                        {
                            min = docWithTotals.DocumentDate.Ticks - aggregationDate.Ticks;
                            dateToReturn = docWithTotals.DocumentDate;
                            isDateFound = true;
                        }
                    }
                }
            }

            return isDateFound ? dateToReturn : (DateTime?)null;
        }

        private IEnumerable<DynamicWrapper> GetAggregationDocumentResult(
            string registerName,
            IEnumerable<FilterCriteria> filter,
            IEnumerable<string> dimensionsProperties,
            IEnumerable<string> valueProperties,
            IEnumerable<AggregationType> aggregationTypes)
        {
            var dimensionsPropertiesArray = dimensionsProperties.ToArray();
            var valuePropertiesArray = valueProperties.ToArray();
            var aggregationTypesArray = aggregationTypes.ToArray();

            if (valuePropertiesArray.Length <= 0)
            {
                throw new ArgumentException($"{nameof(valueProperties)}.Length <= 0");
            }

            if (aggregationTypesArray.Length <= 0)
            {
                throw new ArgumentException($"{nameof(aggregationTypes)}.Length <= 0");
            }

            if (valuePropertiesArray.Length != aggregationTypesArray.Length)
            {
                throw new ArgumentException($"{nameof(valueProperties)}.Length != {aggregationTypes}.Length");
            }

            var groupKey = new DynamicWrapper();

            var groupRequest = new DynamicWrapper
                               {
                                   { "_id", groupKey }
                               };

            foreach (var dimension in dimensionsPropertiesArray)
            {
                groupKey[dimension] = $"${dimension}";
            }

            for (var i = 0; i < valuePropertiesArray.Length; ++i)
            {
                var valueProperty = valuePropertiesArray[i];
                var valueFunction = aggregationTypesArray[i];

                switch (valueFunction)
                {
                    case AggregationType.Count:
                        groupRequest[valueProperty] = new DynamicWrapper { { "$sum", 1 } };
                        break;
                    case AggregationType.Sum:
                        groupRequest[valueProperty]
                            = new DynamicWrapper
                              {
                                  {
                                      // Подсчет суммы с учетом типов записей
                                      "$sum", new DynamicWrapper
                                              {
                                                  {
                                                      // Условное выражение
                                                      "$cond", new object[]
                                                               {
                                                                   // Если запись о расходе
                                                                   new DynamicWrapper
                                                                   {
                                                                       { "$eq", new[] { $"${RegisterConstants.EntryTypeProperty}", RegisterEntryType.Consumption } }
                                                                   },
                                                                   // Значение умножается на -1
                                                                   new DynamicWrapper
                                                                   {
                                                                       { "$multiply", new object[] { -1, $"${valueProperty}" } }
                                                                   },
                                                                   // Значение берется, как есть
                                                                   $"${valueProperty}"
                                                               }
                                                  }
                                              }
                                  }
                              };
                        break;
                    case AggregationType.Avg:
                        groupRequest[valueProperty] = new DynamicWrapper { { "$avg", $"${valueProperty}" } };
                        break;
                    case AggregationType.Max:
                        groupRequest[valueProperty] = new DynamicWrapper { { "$max", $"${valueProperty}" } };
                        break;
                    case AggregationType.Min:
                        groupRequest[valueProperty] = new DynamicWrapper { { "min", $"${valueProperty}" } };
                        break;
                }
            }

            var groupResult = _documentStorageFactory(registerName)
                .Aggregate(filter.ToDocumentStorageFilter())
                .Group(groupRequest)
                .ToList();

            foreach (var grouRow in groupResult)
            {
                foreach (var dimension in dimensionsPropertiesArray)
                {
                    grouRow[dimension] = ((DynamicWrapper)grouRow["_id"])[dimension];
                }

                grouRow["_id"] = null;
            }

            return groupResult;
        }

        private static bool CheckInterval(string interval)
        {
            var validIntervals = new[] { "year", "quarter", "month", "week", "day", "hour", "minute", "second" };

            return validIntervals.Contains(interval.ToLowerInvariant());
        }
    }
}