﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Security;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Security;
using InfinniPlatform.SystemConfig.UserStorage;

namespace InfinniPlatform.RestfulApi.Auth
{
	/// <summary>
	///   Модуль смены пароля пользователя
	/// </summary>
	public sealed class ActionUnitChangePassword
	{
		public void Action(IApplyContext target)
		{
			var storage = new ApplicationUserStorePersistentStorage();

			if (string.IsNullOrEmpty(target.Item.UserName))
			{
				target.CreateValidationMessage("User name not found.",true);
				return;
			}

			if (!string.IsNullOrEmpty(target.Item.UserName) && target.Item.UserName.ToLowerInvariant() == "admin")
			{
				target.CreateValidationMessage("User not found.", true);
				return;
			}



			var user = storage.FindUserByName(target.Item.UserName);
			if (user != null)
			{
				
				if (!new DefaultApplicationUserPasswordHasher().VerifyHashedPassword(user.PasswordHash, target.Item.OldPassword))
				{
					target.CreateValidationMessage("Old password is incorrect.", true);
					return;
				}

				user.PasswordHash = new CustomApplicationUserPasswordHasher(target.Context).HashPassword(target.Item.NewPassword);
				storage.UpdateUser(user);
				//добавляем доступ на чтение пользователей
				target.Context.GetComponent<CachedSecurityComponent>().UpdateAcl();
				target.Result = new DynamicWrapper();
			}
			else
			{
				target.CreateValidationMessage("User name not found.", true);
			}
		}
	}
}
