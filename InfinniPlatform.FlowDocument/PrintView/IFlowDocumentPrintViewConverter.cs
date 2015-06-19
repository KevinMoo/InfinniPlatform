﻿using System.IO;
using InfinniPlatform.Api.PrintView;
using FrameworkFlowDocument = System.Windows.Documents.FlowDocument;

namespace InfinniPlatform.FlowDocument.PrintView
{
    /// <summary>
    ///     Преобразовывает печатное представление в файл определенного формата.
    /// </summary>
    public interface IFlowDocumentPrintViewConverter
    {
        /// <summary>
        ///     Осуществляет преобразование документа печатного представления в файл указанного формата.
        /// </summary>
        /// <param name="printView">Документ печатного представления.</param>
        /// <param name="printViewStream">Поток файла печатного представления.</param>
        /// <param name="printViewFileFormat">Формат файла печатного представления.</param>
        void Convert(FrameworkFlowDocument printView, Stream printViewStream, PrintViewFileFormat printViewFileFormat);
    }
}