﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.RestApi.Auth;
using InfinniPlatform.Core.RestQuery;
using InfinniPlatform.WebApi.ConfigRequestProviders;

using Newtonsoft.Json;

namespace InfinniPlatform.WebApi.Controllers
{
    /// <summary>
    /// REST controller that invokes specified verbs and returns responses
    /// </summary>
    public sealed class StandardApiController : ApiController
    {
        public StandardApiController(IApiControllerFactory apiControllerFactory,
                                     IHttpResultHandlerFactory httpResultHandlerFactory,
                                     IElasticConnection elasticConnection)
        {
            _apiControllerFactory = apiControllerFactory;
            _httpResultHandlerFactory = httpResultHandlerFactory;
            _elasticConnection = elasticConnection;
        }

        private readonly IApiControllerFactory _apiControllerFactory;
        private readonly IHttpResultHandlerFactory _httpResultHandlerFactory;
        private readonly IElasticConnection _elasticConnection;

        private IRestVerbsContainer GetMetadata()
        {
            var metadata = Request.GetRouteData().Values.ContainsKey("metadata") ? _apiControllerFactory.GetTemplate((string)Request.GetRouteData().Values["configuration"], (string)Request.GetRouteData().Values["metadata"]) : null;

            if (metadata == null)
            {
                throw new ArgumentException($"Не найдены метаданные для {Request.GetRouteData().Values["metadata"]}");
            }

            return metadata;
        }

        private string GetServiceName()
        {
            return Request.GetRouteData().Values.ContainsKey("service") ? (string)Request.GetRouteData().Values["service"] : string.Empty;
        }

        [HttpPost]
        public HttpResponseMessage ProcessPostVerb()
        {
            var arguments = JsonConvert.DeserializeObject<IDictionary<string, object>>(Request.Content.ReadAsStringAsync().Result);
            var verbProcessor = GetMetadata().FindVerbPost(GetServiceName(), arguments);

            var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            var result = httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor));

            _elasticConnection.Refresh();

            return result;
        }

        [HttpGet]
        public HttpResponseMessage ProcessGetVerb(string query)
        {
            var arguments = string.IsNullOrEmpty(query) ? new Dictionary<string, object>() : JsonConvert.DeserializeObject<Dictionary<string, object>>(query);
            var verbProcessor = GetMetadata().FindVerbGet(GetServiceName(), arguments);

            var httpResultHandler = _httpResultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            return httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor));
        }

        [HttpPut]
        public HttpResponseMessage ProcessPutVerb()
        {
            throw new ArgumentException("Currently not supported");
        }

        [HttpDelete]
        public HttpResponseMessage ProcessDeleteVerb([FromUri] IEnumerable<string> items)
        {
            throw new ArgumentException("Currently not supported");
        }

        private object InvokeRestVerb(TargetDelegate verbProcessor)
        {
            if (verbProcessor != null)
            {
                SetContext(verbProcessor);
                return verbProcessor.Invoke();
            }
            return null;
        }

        private string GetUserName()
        {
            return (User != null && !string.IsNullOrEmpty(User.Identity.Name))
                ? User.Identity.Name
                : GetServiceName().ToLowerInvariant() == "signin"
                    ? AuthorizationStorageExtensions.AnonymousUser
                    : AuthorizationStorageExtensions.UnknownUser;
        }

        private void SetContext(TargetDelegate invokationInfo)
        {
            var prop = invokationInfo.Target.GetType().GetProperties().FirstOrDefault(p => p.PropertyType.IsAssignableFrom(typeof(IConfigRequestProvider)));
            if (prop != null)
            {
                var requestData = Request.GetRouteData();
                prop.SetValue(invokationInfo.Target, new ConfigRequestProvider
                                                     {
                                                         RequestData = requestData,
                                                         UserName = GetUserName()
                                                     });
            }
        }
    }
}