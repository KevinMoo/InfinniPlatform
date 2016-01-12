﻿using InfinniPlatform.Core.Validation;
using InfinniPlatform.Core.Validation.ObjectValidators;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Validation.ObjectValidators
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class RegexValidatorTest
    {
        private static readonly RegexValidator Validator = new RegexValidator {Pattern = "[0-9]+", Message = "Error"};


        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("Xyz")]
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
        [TestCase("123")]
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