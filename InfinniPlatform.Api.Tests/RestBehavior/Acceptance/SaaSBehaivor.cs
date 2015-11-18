﻿using System;
using System.Linq;

using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Security;
using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Security;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.RestBehavior.Acceptance
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class SaaSBehavior
    {
        private const string ConfigurationId = "TestConfiguration";
        private const string DocumentType = "TestDocument";

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

        private static void CreateUser(string userName, string userPassword, string userTenantId)
        {
            // TODO: Пользователь добавляется напрямую в базу данных, так как на текущий момент в тестовой инфраструктуре очень сложно это сделать по всем "правилам".

            var documentApi = new DocumentApi();
            var passwordHasher = new DefaultApplicationUserPasswordHasher();

            var user = new ApplicationUser
            {
                UserName = userName,
                PasswordHash = passwordHasher.HashPassword(userPassword),
                SecurityStamp = Guid.NewGuid().ToString(),
                Claims = new[]
                                    {
                                        new ApplicationUserClaim
                                        {
                                            Type = new ForeignKey
                                                   {
                                                       Id = AuthorizationStorageExtensions.TenantId,
                                                       DisplayName = AuthorizationStorageExtensions.TenantId
                                                   },
                                            Value = userTenantId
                                        }
                                    }
            };

            documentApi.SetDocument(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore, user);
        }

        [Test]
        public void ShouldReturnOnlyOwnDocuments()
        {
            // Given

            var documentApi = new InfinniDocumentApi(HostingConfig.Default.ServerName, HostingConfig.Default.DefaultServerPort.ToString(), "1");
            var signInApi = new InfinniSignInApi(HostingConfig.Default.ServerName, HostingConfig.Default.DefaultServerPort.ToString(), "1");

            var tenant1 = Guid.NewGuid().ToString("N");
            var tenant2 = Guid.NewGuid().ToString("N");

            var userName1 = $"User_1_{tenant1}";
            var userName2 = $"User_2_{tenant2}";

            var documentUser1 = new { Id = Guid.NewGuid().ToString(), TestProperty = userName1 };
            var documentUser2 = new { Id = Guid.NewGuid().ToString(), TestProperty = userName2 };

            CreateUser(userName1, userName1, tenant1);
            CreateUser(userName2, userName2, tenant2);

            // When

            signInApi.SignInInternal(userName1, userName1);
            documentApi.SetDocument(ConfigurationId, DocumentType, documentUser1);
            signInApi.SignOut();

            signInApi.SignInInternal(userName2, userName2);
            documentApi.SetDocument(ConfigurationId, DocumentType, documentUser2);
            signInApi.SignOut();

            signInApi.SignInInternal(userName1, userName1);
            var allowedDocumentsForUser1 = documentApi.GetDocument(ConfigurationId, DocumentType, s => s.AddCriteria(c => c.Property("TestProperty").IsEquals(userName1)), 0, 10);
            var notAllowedDocumentsForUser1 = documentApi.GetDocument(ConfigurationId, DocumentType, s => s.AddCriteria(c => c.Property("TestProperty").IsEquals(userName2)), 0, 10);
            signInApi.SignOut();

            signInApi.SignInInternal(userName2, userName2);
            var allowedDocumentsForUser2 = documentApi.GetDocument(ConfigurationId, DocumentType, s => s.AddCriteria(c => c.Property("TestProperty").IsEquals(userName2)), 0, 10);
            var notAllowedDocumentsForUser2 = documentApi.GetDocument(ConfigurationId, DocumentType, s => s.AddCriteria(c => c.Property("TestProperty").IsEquals(userName1)), 0, 10);
            signInApi.SignOut();

            // Then

            Assert.IsNotNull(allowedDocumentsForUser1);
            Assert.AreEqual(1, allowedDocumentsForUser1.Count());
            Assert.AreEqual(documentUser1.Id, allowedDocumentsForUser1.ElementAt(0).Id);
            Assert.AreEqual(documentUser1.TestProperty, allowedDocumentsForUser1.ElementAt(0).TestProperty);

            Assert.IsNotNull(notAllowedDocumentsForUser1);
            Assert.AreEqual(0, notAllowedDocumentsForUser1.Count());

            Assert.IsNotNull(allowedDocumentsForUser2);
            Assert.AreEqual(1, allowedDocumentsForUser2.Count());
            Assert.AreEqual(documentUser2.Id, allowedDocumentsForUser2.ElementAt(0).Id);
            Assert.AreEqual(documentUser2.TestProperty, allowedDocumentsForUser2.ElementAt(0).TestProperty);

            Assert.IsNotNull(notAllowedDocumentsForUser2);
            Assert.AreEqual(0, notAllowedDocumentsForUser2.Count());
        }
    }
}