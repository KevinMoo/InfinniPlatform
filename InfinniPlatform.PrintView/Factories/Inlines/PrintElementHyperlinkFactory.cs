﻿using System;

using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Factories.Inlines
{
    internal class PrintElementHyperlinkFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new PrintElementHyperlink();

            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            BuildHelper.ApplyInlineProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyInlineProperties(element, elementMetadata);

            ApplyReference(element, buildContext, elementMetadata);

            // Генерация содержимого элемента

            var inlines = buildContext.ElementBuilder.BuildElements(buildContext, elementMetadata.Inlines);

            if (inlines != null)
            {
                foreach (var inline in inlines)
                {
                    element.Inlines.Add(inline);
                }
            }

            BuildHelper.PostApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.PostApplyTextProperties(element, elementMetadata);

            return element;
        }

        private static void ApplyReference(PrintElementHyperlink element, PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            string referenceSting = BuildHelper.FormatValue(buildContext, elementMetadata.Reference, elementMetadata.SourceFormat);

            if (!string.IsNullOrEmpty(referenceSting))
            {
                Uri referenceUri;

                if (Uri.TryCreate(referenceSting, UriKind.RelativeOrAbsolute, out referenceUri))
                {
                    element.Reference = referenceUri;
                }
            }
        }
    }
}