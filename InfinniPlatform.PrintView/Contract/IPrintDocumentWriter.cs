﻿using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintView.Contract
{
    /// <summary>
    /// Предоставляет метод для записи <see cref="PrintDocument"/> в файл определенного формата.
    /// </summary>
    public interface IPrintDocumentWriter
    {
        /// <summary>
        /// Записывает документ в файл.
        /// </summary>
        /// <param name="stream">Поток для записи в файл.</param>
        /// <param name="document">Документ печатного представления.</param>
        Task Write(Stream stream, PrintDocument document);
    }
}