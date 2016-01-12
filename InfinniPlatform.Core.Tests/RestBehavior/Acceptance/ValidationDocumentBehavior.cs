﻿using System;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.RestApi;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ValidationDocumentBehavior
    {
        private const string ConfigurationId = "TestConfiguration";
        private const string DocumentType = "ValidationDocument";

        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = InfinniPlatformInprocessHost.Start();
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldValidateDocumentWithActionUnit()
        {
            // Given
            var documentApi = new InfinniDocumentApi(HostingConfig.Default.Name, HostingConfig.Default.Port);
            var document = new { Id = Guid.NewGuid(), LastName = "123" };

            // When
            var error = Assert.Catch(() => documentApi.SetDocument(ConfigurationId, DocumentType, document));
            Console.WriteLine(error.ToString());

            // Then
            Assert.IsTrue(error.Message.Contains(@"TestComplexValidatorMessage"));
        }
    }
}