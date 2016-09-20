﻿using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Model.Blocks
{
    internal class PrintElementParagraph : PrintElementBlock
    {
        public PrintElementParagraph()
        {
            Inlines = new List<PrintElementInline>();
        }

        public double? IndentSize { get; set; }

        public List<PrintElementInline> Inlines { get; private set; }
    }
}