﻿using System;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.RestQuery.RestQueryBuilders;
using InfinniPlatform.Api.TestEnvironment;
using NUnit.Framework;

namespace InfinniPlatform.SystemConfig.Tests
{
    [TestFixture]
    [Category(TestCategories.AcceptanceTest)]
    public sealed class ControllerBehavior
    {
        private IDisposable _server;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _server = TestApi.StartServer(c => c.SetHostingConfig(TestSettings.DefaultHostingConfig));

            TestApi.InitClientRouting(TestSettings.DefaultHostingConfig);
        }

        [TestFixtureTearDown]
        public void FixtureTearDown()
        {
            _server.Dispose();
        }

        [Test]
        public void ShouldGetConfigurationHelp()
        {
            var builder = new RestQueryBuilder("SystemConfig", "metadata", "helpconfiguration", null);

            RestQueryResponse help = builder.QueryHelp("dmd");

            Assert.IsNotNull(help);

            // Для того, чтобы следующая проверка прошла, нужно предварительно
            // сгенерировать документацию
            // Assert.True(help.Content.Contains("<html>"));
        }

        [Test]
        public void ShouldGetPrefillUnits()
        {
            RestQueryResponse response = RestQueryApi.QueryPostJsonRaw("systemconfig", "prefill", "getfillitems", null,
                                                                       null);
            Assert.AreEqual(true, response.IsAllOk);
        }


        [Test]
        public void ShouldGetRegisteredConfigList()
        {
            var builder = new RestQueryBuilder( "SystemConfig", "metadata", "getregisteredconfiglist", null);
            RestQueryResponse result = builder.QueryPostJson(null, null);

            Assert.True(result.IsAllOk);

            Assert.IsNotNull(result.ToDynamic().ConfigList);
        }


        [Test]
        public void ShouldGetStandardExtensionPoints()
        {
            var builder = new RestQueryBuilder("SystemConfig", "metadata", "getstandardextensionpoints", null);

            RestQueryResponse response = builder.QueryGet(null, 0, 1000);

            Assert.AreEqual(true, response.IsAllOk);
            Assert.True(response.Content.Contains("[\"Action\",\"OnSuccess\",\"OnFail\",\"Validation\"]"));
        }
    }
}