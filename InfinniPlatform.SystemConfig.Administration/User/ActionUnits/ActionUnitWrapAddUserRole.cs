﻿using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Administration.User.ActionUnits
{
    public sealed class ActionUnitWrapAddUserRole
    {
        public void Action(IApplyContext target)
        {
            dynamic item = target.Item.Document ?? target.Item;

            var api = target.Context.GetComponent<ISecurityComponent>();

            dynamic user = api.Users.FirstOrDefault(u => u.UserName == item.UserName);

            if (user != null)
            {
                //добавляем роль в конфигурации Authorization
                var authApi = target.Context.GetComponent<AuthApi>();

                authApi.AddUserToRole(item.UserName, item.RoleName);

                dynamic documentRole = new DynamicWrapper();
                documentRole.Id = Guid.NewGuid().ToString();
                documentRole.DisplayName = item.RoleName;

                user.UserRoles = user.UserRoles ?? new List<dynamic>();
                user.UserRoles.Add(documentRole);

                target.Context.GetComponent<DocumentApi>().SetDocument("Administration", "User", user);

                target.Result = new DynamicWrapper();
                target.Result.IsValid = true;
                target.Result.ValidationMessage = "User role added.";
            }
            else
            {
                target.Result = new DynamicWrapper();
                target.Result.IsValid = false;
                target.Result.ValidationMessage = "User not found.";
            }
        }
    }
}