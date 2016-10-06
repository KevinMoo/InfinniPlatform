﻿using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Model.Block
{
    /// <summary>
    /// Элемент для создания списка.
    /// </summary>
    public class PrintList : PrintBlock
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "List";


        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintList()
        {
            Items = new List<PrintBlock>();
        }


        /// <summary>
        /// Индекс первого элемента списка.
        /// </summary>
        public int? StartIndex { get; set; }

        /// <summary>
        /// Стиль маркера элементов списка.
        /// </summary>
        public PrintListMarkerStyle? MarkerStyle { get; set; }

        /// <summary>
        /// Отступ содержимого элемента от края маркера.
        /// </summary>
        public double? MarkerOffsetSize { get; set; }

        /// <summary>
        /// Единица измерения отступа содержимого элемента от края маркера.
        /// </summary>
        public PrintSizeUnit? MarkerOffsetSizeUnit { get; set; }

        /// <summary>
        /// Шаблон элементов списка.
        /// </summary>
        public PrintBlock ItemTemplate { get; set; }

        /// <summary>
        /// Список элементов.
        /// </summary>
        public List<PrintBlock> Items { get; set; }
    }
}