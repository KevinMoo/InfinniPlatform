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
        public TenantProvider(IUserIdentityProvider userIdentityProvider)
        {
            _userIdentityProvider = userIdentityProvider;
        }


        private readonly IUserIdentityProvider _userIdentityProvider;


        public string GetTenantId()
        {
            var currentIdentity = GetCurrentIdentity();

            return GetTenantId(currentIdentity);
        }

        public string GetTenantId(IIdentity identity)
        {
            string tenantId;

            if (identity != null && identity.IsAuthenticated)
            {
                // Идентификатор текущей организации
                tenantId = identity.FindFirstClaim(ApplicationClaimTypes.TenantId);

                if (string.IsNullOrEmpty(tenantId))
                {
                    // Идентификатор организации по умолчанию
                    tenantId = identity.FindFirstClaim(ApplicationClaimTypes.DefaultTenantId);

                    if (string.IsNullOrEmpty(tenantId))
                    {
                        // Организация не определена
                        tenantId = SecurityConstants.UndefinedUserTenantId;
                    }
                }
            }
            else
            {
                // Анонимный пользователь
                tenantId = SecurityConstants.AnonymousUserTenantId;
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