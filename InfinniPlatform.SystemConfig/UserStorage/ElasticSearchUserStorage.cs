﻿using System.Linq;

using InfinniPlatform.Core.Security;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.ElasticSearch.ElasticProviders;

namespace InfinniPlatform.SystemConfig.UserStorage
{
    public sealed class ElasticSearchUserStorage
    {
        private readonly ElasticConnection _elasticConnection;
        private readonly IDocumentTransactionScopeProvider _transactionScopeProvider;

        public ElasticSearchUserStorage(ElasticConnection elasticConnection, IDocumentTransactionScopeProvider transactionScopeProvider)
        {
            _elasticConnection = elasticConnection;
            _transactionScopeProvider = transactionScopeProvider;
        }

        public object Find(string property, string value)
        {
            // TODO: Load only UserInfo (without header).
            // TODO: Set specific types, not AllTypes().
            var searchResponse = _elasticConnection.Search<dynamic>(d => d.AllTypes()
                                                                          .Filter(f => f.Term(ElasticConstants.IndexObjectPath + property, value.ToLower()))
                                                                          .Size(1));

            object userInfo = null;

            if (searchResponse.Total > 0 && searchResponse.Documents != null)
            {
                var document = searchResponse.Documents.FirstOrDefault();

                if (document != null)
                {
                    userInfo = document.Values;
                }
            }

            if (userInfo != null)
            {
                var transactionScope = _transactionScopeProvider.GetTransactionScope();

                userInfo = transactionScope.GetDocuments(AuthorizationStorageExtensions.UserStore, new[] { userInfo }).FirstOrDefault();
            }

            return userInfo;
        }

        public void Save(object userId, object userInfo)
        {
            var transactionScope = _transactionScopeProvider.GetTransactionScope();
            transactionScope.Synchronous();
            transactionScope.SaveDocument(AuthorizationStorageExtensions.UserStore, userId, userInfo);
        }

        public void Delete(object userId)
        {
            var transactionScope = _transactionScopeProvider.GetTransactionScope();
            transactionScope.Synchronous();
            transactionScope.DeleteDocument(AuthorizationStorageExtensions.UserStore, userId);
        }
    }
}