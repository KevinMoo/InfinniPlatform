﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Metadata;
using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Api
{
    public class InfinniMetadataApi : BaseApi
    {
        public InfinniMetadataApi(string server, string port, string route) : base(server, port, route)
        {
        }

        /// <summary>
        ///   Добавить метаданные указанного решения
        /// </summary>
        /// <returns>Результат добавления метаданных решения</returns>
        public dynamic InsertSolution(SolutionMetadata solutionMetadata)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataSolution(), solutionMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertSolutionMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанного решения
        /// </summary>
        /// <returns>Результат обновления метаданных решения</returns>
        public dynamic UpdateSolution(SolutionMetadata solutionMetadata)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataSolution(), solutionMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertSolutionMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанного решения
        /// </summary>
        /// <returns>Результат удаления метаданных решения</returns>
        public dynamic DeleteSolution(string version, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataSolutionById(version,name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteSolutionMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные решения по уазанному идентификатору для указанной версии
        /// </summary>
        /// <param name="version">Версия приложения</param>
        /// <param name="name">Наименование приложения</param>
        /// <returns>Метаданные решения</returns>
        public dynamic GetSolution(string version, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataSolutionById(version, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertSolutionMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Добавить метаданные указанной конфигурации
        /// </summary>
        /// <returns>Результат добавления метаданных конфигурации</returns>
        public dynamic InsertConfig(ConfigurationMetadata configMetadata)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataConfig(), configMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertConfigMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанной конфигурации
        /// </summary>
        /// <returns>Результат обновления метаданных конфигурации</returns>
        public dynamic UpdateConfig(ConfigurationMetadata configMetadata)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataConfig(), configMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateConfigMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанной конфигурации
        /// </summary>
        /// <returns>Результат удаления метаданных конфигурации</returns>
        public dynamic DeleteConfig(string version, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataConfigById(version, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteConfigMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные конфигурации по уазанному идентификатору для указанной версии
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование конфигурации</param>
        /// <returns>Метаданные конфигурации</returns>
        public dynamic GetConfig(string version, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataConfigById(version, name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetConfigMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Добавить метаданные указанного меню
        /// </summary>
        /// <returns>Результат добавления метаданных указанного меню</returns>
        public dynamic InsertMenu(MenuMetadata menuMetadata, string version, string configuration)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Menu"), menuMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertMenuMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанного меню
        /// </summary>
        /// <returns>Результат обновления метаданных меню</returns>
        public dynamic UpdateMenu(MenuMetadata menuMetadata, string version, string configuration)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Menu"), menuMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateMenuMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанного меню
        /// </summary>
        /// <returns>Результат удаления метаданных меню</returns>
        public dynamic DeleteMenu(string version, string configuration, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Menu", name ));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteMenuMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные меню по указанному идентификатору
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование меню</param>
        /// <returns>Метаданные меню</returns>
        public dynamic GetMenu(string version, string configuration, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Menu", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetMenuMetadata, response.GetErrorContent()));
        }


        /// <summary>
        ///   Добавить метаданные указанной сборки
        /// </summary>
        /// <returns>Результат добавления метаданных указанной сборки</returns>
        public dynamic InsertAssembly(AssemblyMetadata assemblyMetadata, string version, string configuration)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Assembly"), assemblyMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertAssemblyMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанной сборки
        /// </summary>
        /// <returns>Результат обновления метаданных сборок</returns>
        public dynamic UpdateAssembly(AssemblyMetadata assemblyMetadata, string version, string configuration)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Assembly"), assemblyMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateAssemblyMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанной сборки
        /// </summary>
        /// <returns>Результат удаления метаданных сборки</returns>
        public dynamic DeleteAssembly(string version, string configuration, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Assembly", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteAssemblyMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные сборки по указанному идентификатору
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование сборки</param>
        /// <returns>Метаданные сборки</returns>
        public dynamic GetAssembly(string version, string configuration, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Assembly", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetMenuMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Добавить метаданные указанного регистра
        /// </summary>
        /// <returns>Результат добавления метаданных указанного регистра</returns>
        public dynamic InsertRegister(RegisterMetadata registerMetadata, string version, string configuration)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Register"), registerMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertRegisterMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанного регистра
        /// </summary>
        /// <returns>Результат обновления метаданных регистра</returns>
        public dynamic UpdateRegister(RegisterMetadata registerMetadata, string version, string configuration)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Register"), registerMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateRegisterMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанного регистра
        /// </summary>
        /// <returns>Результат удаления метаданных регистра</returns>
        public dynamic DeleteRegister(string version, string configuration, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Register", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteRegisterMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные регистра по указанному идентификатору
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование регистра</param>
        /// <returns>Метаданные регистра</returns>
        public dynamic GetRegister(string version, string configuration, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Register", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetRegisterMetadata, response.GetErrorContent()));
        }


        /// <summary>
        ///   Добавить метаданные указанного документа
        /// </summary>
        /// <returns>Результат добавления метаданных указанного документа</returns>
        public dynamic InsertDocument(DocumentMetadata documentMetadata, string version, string configuration)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Document"), documentMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertDocumentMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанного документа
        /// </summary>
        /// <returns>Результат обновления метаданных документа</returns>
        public dynamic UpdateDocument(DocumentMetadata documentMetadata, string version, string configuration)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlMetadataElement(version, configuration, "Document"), documentMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateDocumentMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанного документа
        /// </summary>
        /// <returns>Результат удаления метаданных документа</returns>
        public dynamic DeleteDocument(string version, string configuration, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Document", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteDocumentMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные документа
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование документа</param>
        /// <returns>Метаданные документа</returns>
        public dynamic GetDocument(string version, string configuration, string name)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlMetadataElementById(version, configuration, "Document", name));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetDocumentMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Добавить метаданные указанного сценария
        /// </summary>
        /// <returns>Результат добавления метаданных указанного сценария</returns>
        public dynamic InsertScenario(ScenarioMetadata scenarioMetadata, string version, string configuration, string document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Scenario"), scenarioMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertScenarioMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанного сценария
        /// </summary>
        /// <returns>Результат обновления метаданных сценария</returns>
        public dynamic UpdateScenario(ScenarioMetadata scenarioMetadata, string version, string configuration, string document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Scenario"), scenarioMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateScenarioMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанного сценария
        /// </summary>
        /// <returns>Результат удаления метаданных сценария</returns>
        public dynamic DeleteScenario(string version, string configuration, string document, string scenario)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Scenario", scenario));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteScenarioMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные сценария
        /// </summary>
        public dynamic GetScenario(string version, string configuration, string document, string scenario)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Scenario", scenario));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetScenarioMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Добавить метаданные указанного бизнес-процесса
        /// </summary>
        /// <returns>Результат добавления метаданных указанного бизнес-процесса</returns>
        public dynamic InsertProcess(ProcessMetadata processMetadata, string version, string configuration, string document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Process"), processMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertProcessMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанного бизнес-процесса
        /// </summary>
        /// <returns>Результат обновления метаданных бизнес-процесса</returns>
        public dynamic UpdateProcess(ProcessMetadata processMetadata, string version, string configuration, string document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Process"), processMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateProcessMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанного бизнес-процесса
        /// </summary>
        /// <returns>Результат удаления метаданных бизнес-процесса</returns>
        public dynamic DeleteProcess(string version, string configuration, string document, string process)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Process", process));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteProcessMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные бизнес-процесса
        /// </summary>
        public dynamic GetProcess(string version, string configuration, string document, string process)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Process", process));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetProcessMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Добавить метаданные указанного сервиса
        /// </summary>
        /// <returns>Результат добавления метаданных указанного сервиса</returns>
        public dynamic InsertService(ServiceMetadata serviceMetadata, string version, string configuration, string document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Service"), serviceMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertServiceMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанного сервиса
        /// </summary>
        /// <returns>Результат обновления метаданных сервиса</returns>
        public dynamic UpdateService(ServiceMetadata serviceMetadata, string version, string configuration, string document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "Service"), serviceMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateServiceMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанного сервиса
        /// </summary>
        /// <returns>Результат удаления метаданных сервиса</returns>
        public dynamic DeleteService(string version, string configuration, string document, string service)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Service", service));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteServiceMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные сервиса
        /// </summary>
        public dynamic GetService(string version, string configuration, string document, string service)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "Service", service));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetServiceMetadata, response.GetErrorContent()));
        }


        /// <summary>
        ///   Добавить метаданные указанного представления
        /// </summary>
        /// <returns>Результат добавления метаданных указанного представления</returns>
        public dynamic InsertView(ViewMetadata viewMetadata, string version, string configuration, string document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "View"), viewMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertViewMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанного представления
        /// </summary>
        /// <returns>Результат обновления метаданных представления</returns>
        public dynamic UpdateView(ViewMetadata viewMetadata, string version, string configuration, string document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "View"), viewMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdateViewMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанного представления
        /// </summary>
        /// <returns>Результат удаления метаданных представления</returns>
        public dynamic DeleteView(string version, string configuration, string document, string view)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "View", view));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeleteViewMetadaa, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные представления
        /// </summary>
        public dynamic GetView(string version, string configuration, string document, string view)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "View", view));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetViewMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Добавить метаданные указанного представления печатной формы
        /// </summary>
        /// <returns>Результат добавления метаданных указанного представления печатной формы</returns>
        public dynamic InsertPrintView(PrintViewMetadata printViewMetadata, string version, string configuration, string document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPut(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "PrintView"), printViewMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInsertPrintViewMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Обновить метаданные указанного представления печатной формы
        /// </summary>
        /// <returns>Результат обновления метаданных представления печатной формы</returns>
        public dynamic UpdatePrintView(PrintViewMetadata printViewMetadata, string version, string configuration, string document)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElement(version, configuration, document, "PrintView"), printViewMetadata);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToUpdatePrintViewMetaata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Удалить метаданные указанного представления печатной формы
        /// </summary>
        /// <returns>Результат удаления метаданных представления печатной формы</returns>
        public dynamic DeletePrintView(string version, string configuration, string document, string printView)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryDelete(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "PrintView", printView));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToDeletePrintViewMetadata, response.GetErrorContent()));
        }

        /// <summary>
        ///   Получить метаданные представления печатной формы
        /// </summary>
        public dynamic GetPrintView(string version, string configuration, string document, string printView)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlDocumentMetadataElementById(version, configuration, document, "PrintView", printView));

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToGetPrintViewMetadata, response.GetErrorContent()));
        }

    }
}
