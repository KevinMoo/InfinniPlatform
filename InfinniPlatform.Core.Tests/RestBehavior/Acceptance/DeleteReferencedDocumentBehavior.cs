﻿using System;
using System.Linq;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class DeleteReferencedDocumentBehavior
    {
        private const string MainDocumentType = "DeleteReferencedDocument";
        private const string ReferenceDocumentType = "AddressDocument";

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
        public void ShouldDeleteDocumentWithReferenceCorrectly()
        {
            // Given

            var documentApi = new DocumentApiClient(HostingConfig.Default.Name, HostingConfig.Default.Port, true);

            var mainDocumentId = Guid.NewGuid();
            var referenceDocumentId = Guid.NewGuid();

            var mainDocument = new
            {
                Id = mainDocumentId,
                Name = "Ivanov",
                Address = new
                {
                    Id = referenceDocumentId,
                    DisplayName = "Lenina"
                }
            };

            var referenceDocument = new
            {
                Id = referenceDocumentId,
                Street = "Lenina"
            };

            // When

            documentApi.SetDocument(ReferenceDocumentType, referenceDocument);
            documentApi.SetDocument(MainDocumentType, mainDocument);

            var mainDocumentBeforeDelete = documentApi.GetDocument(MainDocumentType, filter => filter.AddCriteria(c => c.Property("Id").IsEquals(mainDocumentId)), 0, 1);
            var referenceDocumentBeforeDelete = documentApi.GetDocument(ReferenceDocumentType, filter => filter.AddCriteria(c => c.Property("Id").IsEquals(referenceDocumentId)), 0, 1);

            documentApi.DeleteDocument(MainDocumentType, mainDocumentId.ToString());

            var mainDocumentAfterDelete = documentApi.GetDocument(MainDocumentType, filter => filter.AddCriteria(c => c.Property("Id").IsEquals(mainDocumentId)), 0, 1);
            var referenceDocumentAfterDelete = documentApi.GetDocument(ReferenceDocumentType, filter => filter.AddCriteria(c => c.Property("Id").IsEquals(referenceDocumentId)), 0, 1);

            // Then

            Assert.IsNotNull(mainDocumentBeforeDelete);
            Assert.AreEqual(1, mainDocumentBeforeDelete.Count());
            Assert.IsNotNull(referenceDocumentBeforeDelete);
            Assert.AreEqual(1, referenceDocumentBeforeDelete.Count());

            Assert.IsNotNull(mainDocumentAfterDelete);
            Assert.AreEqual(0, mainDocumentAfterDelete.Count());
            Assert.IsNotNull(referenceDocumentAfterDelete);
            Assert.AreEqual(1, referenceDocumentAfterDelete.Count());
        }
    }
}