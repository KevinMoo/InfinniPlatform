﻿namespace InfinniPlatform.PrintView.Factories
{
    /// <summary>
    /// Фабрика для создания элемента печатного представления на основе метаданных.
    /// </summary>
    internal interface IPrintElementFactory
    {
        object Create(PrintElementBuildContext buildContext, dynamic elementMetadata);
    }
}