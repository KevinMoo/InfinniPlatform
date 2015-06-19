﻿using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
    public sealed class ActionUnitChangePassword
    {
        public void Action(IApplyContext target)
        {
            target.Context.GetComponent<SignInApi>(target.Version)
                .ChangePassword(target.Item.Document.UserName, target.Item.Document.OldPassword,
                    target.Item.Document.NewPassword);
            target.Result = new DynamicWrapper();
        }
    }
}