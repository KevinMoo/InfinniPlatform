﻿using System;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Sdk.Documents.Services
{
    /// <summary>
    /// Обработчик для сервиса по работе с документами.
    /// </summary>
    /// <typeparam name="TDocument">Тип документа.</typeparam>
    public interface IDocumentHttpService<TDocument>
    {
        /// <summary>
        /// Имя типа документа.
        /// </summary>
        string DocumentType { get; }


        /// <summary>
        /// Разрешено ли получение документов.
        /// </summary>
        bool CanGet { get; }

        /// <summary>
        /// Вызывается перед получением документов.
        /// </summary>
        /// <param name="query">Запрос на получение документов.</param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        DocumentServiceResult<DocumentGetQueryResult> OnBeforeGet(DocumentGetQuery<TDocument> query);

        /// <summary>
        /// Вызывается после получения документов.
        /// </summary>
        /// <param name="query">Запрос на получение документов.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        void OnAfterGet(DocumentGetQuery<TDocument> query, DocumentServiceResult<DocumentGetQueryResult> result, Exception exception);


        /// <summary>
        /// Разрешено ли сохранение документов.
        /// </summary>
        bool CanPost { get; }

        /// <summary>
        /// Вызывается перед сохранением документов.
        /// </summary>
        /// <param name="query">Запрос на сохранение документа.</param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        DocumentServiceResult<DocumentPostQueryResult> OnBeforePost(DocumentPostQuery<TDocument> query);

        /// <summary>
        /// Вызывается после сохранения документов.
        /// </summary>
        /// <param name="query">Запрос на сохранение документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        void OnAfterPost(DocumentPostQuery<TDocument> query, DocumentServiceResult<DocumentPostQueryResult> result, Exception exception);


        /// <summary>
        /// Разрешено ли удаление документов.
        /// </summary>
        bool CanDelete { get; }

        /// <summary>
        /// Вызывается перед удалением документов.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>Результат обработки запроса или <c>null</c>, если запрос не был обработан.</returns>
        DocumentServiceResult<DocumentDeleteQueryResult> OnBeforeDelete(DocumentDeleteQuery query);

        /// <summary>
        /// Вызывается после удаления документов.
        /// </summary>
        /// <param name="query">Запрос на удаление документа.</param>
        /// <param name="result">Результат обработки запроса.</param>
        /// <param name="exception">Исключение обработки запроса.</param>
        void OnAfterDelete(DocumentDeleteQuery query, DocumentServiceResult<DocumentDeleteQueryResult> result, Exception exception);


        /// <summary>
        /// Загружает модуль.
        /// </summary>
        /// <param name="builder">Регистратор обработчиков запросов.</param>
        void Load(IHttpServiceBuilder builder);
    }
}