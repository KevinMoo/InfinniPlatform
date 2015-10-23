﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.Transactions;
using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Transactions;

namespace InfinniPlatform.Transactions
{
    /// <summary>
    /// Мастер-транзакция, отвечающая за сохранение данных по завершении
    /// </summary>
    public sealed class TransactionMaster : ITransaction
    {
        /// <summary>
        /// Конструктор мастер-транзакции
        /// </summary>
        /// <param name="indexFactory">Фабрика для работы с индексами</param>
        /// <param name="blobStorageFactory">Фабрика хранилища бинарных данных</param>
        /// <param name="transactionMarker">Идентификатор создаваемой транзакции</param>
        /// <param name="itemsList">Разделяемый между различными экземплярами ITransaction список присоединенных элементов</param>
        public TransactionMaster(IIndexFactory indexFactory, IBlobStorageFactory blobStorageFactory, string transactionMarker, List<AttachedInstance> itemsList)
        {
            _indexFactory = indexFactory;
            _blobStorageFactory = blobStorageFactory;
            _transactionMarker = transactionMarker;
            _itemsList = itemsList;
        }


        private readonly IBlobStorageFactory _blobStorageFactory;
        private readonly IIndexFactory _indexFactory;
        private readonly List<AttachedInstance> _itemsList;
        private readonly string _transactionMarker;


        public Action<ITransaction> OnCommit { get; set; }


        /// <summary>
        /// Зафиксировать транзакцию
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                // Насколько я понял, внутри этой велосипедной транзакции накапливаются документы, которые нужно сохранить.
                // Сначала эти документы сохраняются в базу ElasticSearch, затем, если документы ссылаются на какие-то BLOB,
                // происходит последовательное сохранение этих BLOB.

                foreach (var item in _itemsList.Where(i => !i.Detached))
                {
                    var versionProvider = _indexFactory.BuildVersionProvider(item.ConfigId, item.DocumentId, item.Routing, item.Version);

                    versionProvider.SetDocuments(item.Documents);

                    if (item.Files != null && item.Files.Count > 0)
                    {
                        var blobStorage = _blobStorageFactory.CreateBlobStorage();
                        var binaryManager = new BinaryManager(blobStorage);

                        foreach (var fileDescription in item.Files)
                        {
                            binaryManager.SaveBinary(item.Documents, item.ConfigId, item.Version, item.DocumentId, fileDescription.FieldName, fileDescription.Bytes);
                        }
                    }
                }

                if (OnCommit != null)
                {
                    OnCommit(this);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Fail to commit transaction: " + e, e);
            }
        }

        /// <summary>
        /// Отсоединить документ от транзакции
        /// </summary>
        /// <param name="instanceId">Идентификатор отсоединяемого документа</param>
        public void Detach(string instanceId)
        {
            var itemDetached = _itemsList.FirstOrDefault(i => i.Documents.Any(a => a.Id.Equals(instanceId)));
            if (itemDetached != null)
            {
                itemDetached.Detached = true;
            }
        }

        /// <summary>
        /// Присоединить файл к участнику транзакции, ссылающемуся на документ
        /// с указанным идентификатором
        /// </summary>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Поле ссылки в документе</param>
        /// <param name="stream">Файловый поток</param>
        public void AttachFile(string instanceId, string fieldName, Stream stream)
        {
            var attachedInstance = _itemsList.FirstOrDefault(i => i.ContainsInstance(instanceId));

            if (attachedInstance != null)
            {
                attachedInstance.AddFile(fieldName, stream);
            }
            else
            {
                throw new ArgumentException(string.Format(Resources.InstanceNotFoundToAttachFile, instanceId));
            }
        }

        /// <summary>
        /// Отсоединить файл от участника транзакции, ссылающегося на документ
        /// с указанным идентификатором
        /// </summary>
        /// <param name="instanceId">Идентификатор документа</param>
        /// <param name="fieldName">Наименование поля бинарных данных в схеме документа</param>
        public void DetachFile(string instanceId, string fieldName)
        {
            var attachedInstance = _itemsList.FirstOrDefault(i => i.ContainsInstance(instanceId));
            if (attachedInstance != null)
            {
                attachedInstance.RemoveFile(fieldName);
            }
        }

        /// <summary>
        /// Присоединить документ к транзакции
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор типа документа</param>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="document">Присоединяемые документы</param>
        /// <param name="routing">Роутинг сохранения</param>
        public void Attach(string configId, string documentId, string version, IEnumerable<dynamic> document,
                           string routing)
        {
            _itemsList.Add(new AttachedInstance
                           {
                               Documents = document,
                               ConfigId = configId,
                               DocumentId = documentId,
                               Version = version,
                               Routing = routing
                           });
        }

        /// <summary>
        /// Главная транзакция
        /// </summary>
        public ITransaction MasterTransaction
        {
            get { return this; }
        }

        /// <summary>
        /// Получить идентификатор транзакции
        /// </summary>
        /// <returns></returns>
        public string GetTransactionMarker()
        {
            return _transactionMarker;
        }

        /// <summary>
        /// Получить список документов транзакции
        /// </summary>
        /// <returns>Список документов транзакции</returns>
        public List<AttachedInstance> GetTransactionItems()
        {
            return _itemsList;
        }
    }
}