﻿using System.Drawing;

using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Inline
{
    [TestFixture]
    [Platform(Exclude = "Mono")]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintBarcodeEan13FactoryTest
    {
        private const string BarcodeEan13Text = "123456789012";

        [Test]
        public void ShouldApplyText()
        {
            // Given

            var expectedImage = Resources.BarcodeEan13Rotate0;

            var template = new PrintBarcodeEan13
                           {
                               ShowText = false,
                               Text = BarcodeEan13Text
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Size);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyTextFromSource()
        {
            // Given

            var expectedImage = Resources.BarcodeEan13Rotate0;

            var template = new PrintBarcodeEan13
                           {
                               ShowText = false,
                               Source = "$"
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template, BarcodeEan13Text);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Size);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyRotation0()
        {
            // Given

            var expectedImage = Resources.BarcodeEan13Rotate0;

            var template = new PrintBarcodeEan13
                           {
                               ShowText = false,
                               Text = BarcodeEan13Text,
                               Rotation = PrintImageRotation.Rotate0
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Size);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        [Repeat(2)] // TODO
        public void ShouldApplyRotation90()
        {
            // Given

            var expectedImage = Resources.BarcodeEan13Rotate90;

            var template = new PrintBarcodeEan13
                           {
                               ShowText = false,
                               Text = BarcodeEan13Text,
                               Rotation = PrintImageRotation.Rotate90
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Size);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyRotation180()
        {
            // Given

            var expectedImage = Resources.BarcodeEan13Rotate180;

            var template = new PrintBarcodeEan13
                           {
                               ShowText = false,
                               Text = BarcodeEan13Text,
                               Rotation = PrintImageRotation.Rotate180
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Size);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyRotation270()
        {
            // Given

            var expectedImage = Resources.BarcodeEan13Rotate270;

            var template = new PrintBarcodeEan13
                           {
                               ShowText = false,
                               Text = BarcodeEan13Text,
                               Rotation = PrintImageRotation.Rotate270
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Size);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }
    }
}