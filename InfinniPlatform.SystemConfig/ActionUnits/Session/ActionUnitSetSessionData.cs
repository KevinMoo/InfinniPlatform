﻿using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Session;

namespace InfinniPlatform.SystemConfig.ActionUnits.Session
{
    public sealed class ActionUnitSetSessionData
    {
        public ActionUnitSetSessionData(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        private readonly ISessionManager _sessionManager;

        public void Action(IApplyContext target)
        {
            _sessionManager.SetSessionData(target.Item.ClaimType, target.Item.ClaimValue);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
        }
    }
}