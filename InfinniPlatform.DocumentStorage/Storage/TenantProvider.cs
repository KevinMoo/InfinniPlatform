﻿using System.Security.Principal;

using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Session;

namespace InfinniPlatform.DocumentStorage.Storage
{
    /// <summary>
    /// Предоставляет методы определения пользователя системы по модели SaaS.
    /// </summary>
    internal class TenantProvider : ITenantProvider
    {
        private const string TenantId = "tenantid";
        private const string AnonymousUser = "anonymous";
        private const string DefaultTenantId = "defaulttenantid";


        public TenantProvider(ISessionManager sessionManager, IUserIdentityProvider userIdentityProvider)
        {
            _sessionManager = sessionManager;
            _userIdentityProvider = userIdentityProvider;
        }


        private readonly ISessionManager _sessionManager;
        private readonly IUserIdentityProvider _userIdentityProvider;


        public string GetTenantId()
        {
            var currentIdentity = GetCurrentIdentity();

            return GetTenantId(currentIdentity);
        }

        public string GetTenantId(IIdentity identity)
        {
            string tenantId = null;

            if (identity != null)
            {
                var sessionManager = _sessionManager;

                if (sessionManager != null)
                {
                    tenantId = sessionManager.GetSessionData(TenantId);
                }

                if (string.IsNullOrEmpty(tenantId))
                {
                    tenantId = identity.FindFirstClaim(DefaultTenantId);

                    if (string.IsNullOrEmpty(tenantId))
                    {
                        tenantId = identity.FindFirstClaim(TenantId);
                    }
                }
            }

            if (string.IsNullOrEmpty(tenantId))
            {
                tenantId = AnonymousUser;
            }

            return tenantId;
        }


        private IIdentity GetCurrentIdentity()
        {
            var currentIdentity = _userIdentityProvider.GetUserIdentity();
            var currentUserId = currentIdentity.GetUserId();
            var isNotAuthenticated = string.IsNullOrEmpty(currentUserId);
            return isNotAuthenticated ? null : currentIdentity;
        }
    }
}