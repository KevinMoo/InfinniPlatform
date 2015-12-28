﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

using InfinniPlatform.Api.Security;
using InfinniPlatform.Sdk.ContextComponents;

namespace InfinniPlatform.Transactions
{
    /// <summary>
    /// Предоставляет методы определения пользователя системы по модели SaaS.
    /// </summary>
    internal class TenantProvider : ITenantProvider
    {
        private const string TenantId = "tenantid";
        private const string AnonymousUser = "anonimous";
        private const string DefaultTenantId = "defaulttenantid";

        private static readonly string[] SystemConfigurations =
        {
            "administration",
            "administrationcustomization",
            "authorization",
            "restfulapi",
            "systemconfig",
            "update"
        };


        public TenantProvider(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }


        private readonly ISessionManager _sessionManager;


        public string GetTenantId(string indexName = null)
        {
            string tenantId = null;

            if (indexName != null && SystemConfigurations.Contains(indexName, StringComparer.OrdinalIgnoreCase))
            {
                tenantId = AnonymousUser;
            }
            else
            {
                var currentIdentity = GetCurrentIdentity();

                if (currentIdentity != null)
                {
                    var sessionManager = _sessionManager;

                    if (sessionManager != null)
                    {
                        tenantId = sessionManager.GetSessionData(TenantId);
                    }

                    if (string.IsNullOrEmpty(tenantId))
                    {
                        tenantId = currentIdentity.FindFirstClaim(DefaultTenantId);

                        if (string.IsNullOrEmpty(tenantId))
                        {
                            tenantId = currentIdentity.FindFirstClaim(TenantId);
                        }
                    }
                }

                if (string.IsNullOrEmpty(tenantId))
                {
                    tenantId = AnonymousUser;
                }
            }

            return tenantId;
        }

        private static IIdentity GetCurrentIdentity()
        {
            var currentIdentity = Thread.CurrentPrincipal?.Identity;
            var currentUserId = currentIdentity?.FindFirstClaim(ClaimTypes.NameIdentifier);
            var isNotAuthenticated = string.IsNullOrEmpty(currentUserId);
            return isNotAuthenticated ? null : currentIdentity;
        }
    }
}