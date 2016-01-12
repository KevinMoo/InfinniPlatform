﻿using InfinniPlatform.Core.Validation;
using InfinniPlatform.Core.Validation.ObjectValidators;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Validation.ObjectValidators
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class StartsWithValidatorTest
    {
        private static readonly StartsWithValidator Validator = new StartsWithValidator
            {
                Value = "Abc",
                Message = "Error"
            };


        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("Xyz")]
        [TestCase("XyAbcz")]
        [TestCase("Xyabcz")]
        [TestCase("XyzAbc")]
        [TestCase("Xyzabc")]
        public void ShouldValidateWhenFailure(string validationObject)
        {
            // When
            var result = new ValidationResult();
            bool isValid = Validator.Validate(validationObject, result);

            // Then
            Assert.IsFalse(isValid);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Items != null && result.Items.Count == 1);
            Assert.AreEqual(Validator.Message, result.Items[0].Message);
        }


        [Test]
        [TestCase("Abc")]
        [TestCase("abc")]
        [TestCase("AbcXyz")]
        [TestCase("abcXyz")]
        public void ShouldValidateWhenSuccess(string validationObject)
        {
            // When
            var result = new ValidationResult();
            bool isValid = Validator.Validate(validationObject, result);

            // Then
            Assert.IsTrue(isValid);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Items == null || result.Items.Count == 0);
        }
    }
}