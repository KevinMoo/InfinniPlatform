﻿using InfinniPlatform.Core.Security;
using InfinniPlatform.Sdk.Security;

using Microsoft.AspNet.Identity;

namespace InfinniPlatform.Authentication.InternalIdentity
{
    /// <summary>
    /// Сведения о пользователе системы с реализацией интерфейса <see cref="IUser" />.
    /// </summary>
    internal sealed class IdentityApplicationUser : ApplicationUser, IUser
    {
    }
}