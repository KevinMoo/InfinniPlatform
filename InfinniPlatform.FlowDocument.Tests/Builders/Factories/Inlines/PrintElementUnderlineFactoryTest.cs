﻿using System.Windows.Documents;
using InfinniPlatform.Sdk.Application.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementUnderlineFactoryTest
    {
        [Test]
        public void ShouldBuildInlines()
        {
            // Given

            dynamic inline1 = new DynamicWrapper();
            inline1.Run = new DynamicWrapper();
            inline1.Run.Text = "Inline1";

            dynamic inline2 = new DynamicWrapper();
            inline2.Run = new DynamicWrapper();
            inline2.Run.Text = "Inline2";

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Inlines = new[] {inline1, inline2};

            // When
            Underline element = BuildTestHelper.BuildUnderline(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Inlines);
            Assert.AreEqual(2, element.Inlines.Count);
            Assert.IsInstanceOf<Run>(element.Inlines.FirstInline);
            Assert.IsInstanceOf<Run>(element.Inlines.LastInline);
            Assert.AreEqual("Inline1", ((Run) element.Inlines.FirstInline).Text);
            Assert.AreEqual("Inline2", ((Run) element.Inlines.LastInline).Text);
        }
    }
}