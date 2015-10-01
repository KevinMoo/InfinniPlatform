﻿using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Views
{
    public class PrintViewDocument : PrintElement
    {
        public PrintViewDocument()
        {
            Styles = new List<PrintViewStyle>();
            Blocks = new List<PrintElementBlock>();
        }

        public PrintElementSize PageSize { get; set; }
        public PrintElementThickness PagePadding { get; set; }
        public List<PrintViewStyle> Styles { get; private set; }
        public List<PrintElementBlock> Blocks { get; private set; }
    }
}