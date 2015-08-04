﻿using System.IO;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementImageHtmlConverter : IHtmlBuilderBase<PrintElementImage>
    {
        public override void Build(HtmlBuilderContext context, PrintElementImage element, TextWriter result)
        {
            result.Write("<img src=\"data:image/png;base64,");
            result.StreamToBase64(element.Source);

            result.Write("\" style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyInlineStyles(element);
            result.ApplyImageStyles(element);

            result.Write("\">");

            result.Write("</img>");
        }
    }
}
