﻿using InfinniPlatform.Core.Factories;
using InfinniPlatform.Core.PrintView;
using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.PrintView;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Reporting.PrintView
{
    /// <summary>
    /// Фабрика для создания построителя печатного представления на основе System.Windows.Documents.FlowDocument.
    /// </summary>
    public sealed class FlowDocumentPrintViewBuilderFactory : IPrintViewBuilderFactory
    {
        public FlowDocumentPrintViewBuilderFactory(IAppConfiguration appConfiguration)
        {
            var settings = appConfiguration.GetSection<PrintViewSettings>(PrintViewSettings.SectionName);

            var printViewConverter = new FlowDocumentPrintViewConverter(settings);
            var printViewFactory = new FlowDocumentPrintViewFactory();

            _printViewBuilder = new FlowDocumentPrintViewBuilder(printViewFactory, printViewConverter);
        }


        private readonly IPrintViewBuilder _printViewBuilder;


        public IPrintViewBuilder CreatePrintViewBuilder()
        {
            return _printViewBuilder;
        }
    }
}