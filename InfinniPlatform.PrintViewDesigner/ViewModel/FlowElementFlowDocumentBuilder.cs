﻿using System.IO;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.Model.Views;

using FrameworkFlowDocument = System.Windows.Documents.FlowDocument;

namespace InfinniPlatform.PrintViewDesigner.ViewModel
{
    sealed class FlowElementFlowDocumentBuilder : IFlowElementBuilderBase<PrintViewDocument>
    {

        public override object Build(FlowElementBuilderContext context, PrintViewDocument element, PrintElementMetadataMap elementMetadataMap)
        {
            var elementContent = new FrameworkFlowDocument
            {
                FontFamily = new FontFamily("Arial")
            };

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyDocumentStyles(elementContent, element);

            foreach (var block in element.Blocks)
            {
                var blockContent = context.Build<Block>(block, elementMetadataMap);

                elementContent.Blocks.Add(blockContent);
            }

            using (var w = File.Create("C:\\Test.xaml"))
            {
                XamlWriter.Save(elementContent, w);
                w.Flush();
            }

            return elementContent;
        }
    }
}
