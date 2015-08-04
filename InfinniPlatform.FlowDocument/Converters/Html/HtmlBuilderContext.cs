﻿using System;
using System.Collections.Generic;
using System.Text;

using InfinniPlatform.FlowDocument.Model;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    public class HtmlBuilderContext
    {
        private readonly Dictionary<Type, IHtmlBuilder> _builders
            = new Dictionary<Type, IHtmlBuilder>();

        public HtmlBuilderContext Register<TElement, TBuilder>()
            where TElement : PrintElement
            where TBuilder : IHtmlBuilderBase<TElement>, new()
        {
            _builders[typeof(TElement)] = new TBuilder();

            return this;
        }

        public void Build(PrintElement element, StringBuilder result)
        {
            if (element != null)
            {
                IHtmlBuilder builder;

                if (_builders.TryGetValue(element.GetType(), out builder))
                {
                    builder.Build(this, element, result);
                }
            }
        }
    }
}