﻿using InfinniPlatform.PrintView.Model.Blocks;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Writers.Html.Blocks
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementLineHtmlBuilderTest
    {
        [Test]
        public void ShouldBuildLine()
        {
            // Given
            var context = HtmlBuilderTestHelper.CreateHtmlBuilderContext();
            var element = new PrintElementLine();
            var result = new TextWriterWrapper();

            // When
            context.Build(element, result.Writer);

            // Then
            Assert.AreEqual(Resources.ResultTestShouldBuildLine, result.GetText());
        }
    }
}