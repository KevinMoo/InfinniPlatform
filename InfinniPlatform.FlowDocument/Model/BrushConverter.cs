﻿namespace InfinniPlatform.FlowDocument.Model
{
    public sealed class BrushConverter
    {
        public Brush ConvertFromString(string valueString)
        {
            return new Brush(valueString);
        }
    }
}