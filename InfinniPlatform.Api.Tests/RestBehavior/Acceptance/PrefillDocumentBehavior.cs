﻿using System;

using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.NodeServiceHost;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class PrefillDocumentBehavior
    {
        private const string ConfigurationId = "TestConfiguration";
        private const string DocumentType = "PrefillDocument";

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
        public void ShouldPrefillDocumentBySchema()
        {
            // Given
            var documentApi = new DocumentApi();

            // When
            var document = documentApi.CreateDocument(ConfigurationId, DocumentType);

            // Then
            Assert.IsNotNull(document);
            Assert.AreEqual("ИВАНОВ", document.Name);
            Assert.AreEqual("2015-01-01", document.ObservationDate);
            Assert.AreEqual("TestValue", document.PrefiledField);
        }
    }
}