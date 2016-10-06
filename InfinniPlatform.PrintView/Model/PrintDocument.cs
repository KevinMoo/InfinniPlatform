﻿using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Model
{
    /// <summary>
    /// Документ печатного представления.
    /// </summary>
    public class PrintDocument : PrintElement
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintDocument()
        {
            Styles = new List<PrintStyle>();
            Blocks = new List<PrintBlock>();
        }


        /// <summary>
        /// Размеры страницы.
        /// </summary>
        public PrintSize PageSize { get; set; }

        /// <summary>
        /// Отступ от края страницы до содержимого страницы.
        /// </summary>
        public PrintThickness PagePadding { get; set; }

        /// <summary>
        /// Список стилей.
        /// </summary>
        public List<PrintStyle> Styles { get; set; }

        /// <summary>
        /// Список элементов.
        /// </summary>
        public List<PrintBlock> Blocks { get; set; }
    }
}