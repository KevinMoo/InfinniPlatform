﻿using InfinniPlatform.PrintView.Model.Inlines;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementItalicHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildItalic()
        {
            //Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementItalic();
            var result = new TextWriterWrapper();

            var run = new PrintElementRun {Text = "Italic Text."};

            //When
            element.Inlines.Add(run);

            context.Build(element, result.Writer);

            //Then
            Assert.AreEqual(Resources.ResultTestShouldBuildItalic, result.GetText());
        }
    }
}