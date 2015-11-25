﻿using InfinniPlatform.Api.PrintView;
using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    /// Компонент для получения печатных форм из контекста
    /// </summary>
    public sealed class PrintViewComponent : IPrintViewComponent
    {
        public PrintViewComponent(IPrintViewBuilderFactory printViewBuilderFactory)
        {
            _printViewBuilder = printViewBuilderFactory.CreatePrintViewBuilder();
        }

        private readonly IPrintViewBuilder _printViewBuilder;

        public byte[] BuildPrintView(object printView, object printViewSource, PrintViewFileFormat printViewFileFormat = PrintViewFileFormat.Pdf)
        {
            return _printViewBuilder.BuildFile(printView, printViewSource, printViewFileFormat);
        }
    }
}