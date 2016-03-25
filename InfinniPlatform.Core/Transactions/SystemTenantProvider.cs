﻿namespace InfinniPlatform.Core.Transactions
{
    internal class SystemTenantProvider : ISystemTenantProvider
    {
        private const string SystemTenant = "system";

        public string GetTenantId()
        {
            return SystemTenant;
        }
    }
}