﻿namespace InfinniPlatform.PrintView.Model.Inline
{
    /// <summary>
    /// Уровень защиты от ошибок штрих-кода в формате QR.
    /// </summary>
    public enum PrintBarcodeQrErrorCorrection
    {
        /// <summary>
        /// 7% данных могут быть восстановлены.
        /// </summary>
        Low,

        /// <summary>
        /// 15% данных могут быть восстановлены.
        /// </summary>
        Medium,

        /// <summary>
        /// 25% данных могут быть восстановлены.
        /// </summary>
        Quartile,

        /// <summary>
        /// 30% данных могут быть восстановлены.
        /// </summary>
        High
    }
}