﻿using System;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Auth.Identity.UserCache;
using InfinniPlatform.DocumentStorage;
using Microsoft.AspNetCore.Identity;

namespace InfinniPlatform.Auth.Identity.DocumentStorage
{
    /// <summary>
    /// Хранилище пользователей.
    /// </summary>
    /// <typeparam name="TUser">Пользователь.</typeparam>
    public partial class UserStore<TUser> : IUserStore<TUser> where TUser : AppUser
    {
        protected readonly UserCache<AppUser> UserCache;
        protected readonly Lazy<ISystemDocumentStorage<TUser>> Users;

        public UserStore(ISystemDocumentStorageFactory documentStorageFactory, UserCache<AppUser> userCache)
        {
            Users = new Lazy<ISystemDocumentStorage<TUser>>(() => documentStorageFactory.GetStorage<TUser>());
            UserCache = userCache;
        }

        public void Dispose()
        {
        }

        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken token)
        {
            await Users.Value.InsertOneAsync(user);

            UpdateUserInCache(user);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken token)
        {
            await Users.Value.ReplaceOneAsync(user, u => u.Id == user.Id);

            UpdateUserInCache(user);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken token)
        {
            await Users.Value.DeleteOneAsync(u => u.Id == user.Id);

            RemoveUserFromCache(user.Id);

            return IdentityResult.Success;
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.UserName);
        }

        public async Task SetUserNameAsync(TUser user, string userName, CancellationToken token)
        {
            user.UserName = userName;

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken token)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public async Task SetNormalizedUserNameAsync(TUser user, string normalizedUserName, CancellationToken token)
        {
            user.NormalizedUserName = normalizedUserName;

            await Users.Value.ReplaceOneAsync(user);
            UpdateUserInCache(user);
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken token)
        {
            return await FindUserInCache(() => (TUser) UserCache.FindUserById(userId),
                                         async () => await Users.Value.Find(u => u._id.Equals(userId)).FirstOrDefaultAsync());
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken token)
        {
            return await FindUserInCache(() => (TUser) UserCache.FindUserByUserName(normalizedUserName),
                                         async () => await Users.Value.Find(u => u.UserName == normalizedUserName.ToLower()).FirstOrDefaultAsync());
        }

        /// <summary>
        /// Обновляет сведения о пользователе в локальном кэше.
        /// </summary>
        protected void UpdateUserInCache(AppUser user)
        {
            UserCache.AddOrUpdateUser(user);
        }

        /// <summary>
        /// Удаляет сведения о пользователе из локального кэша.
        /// </summary>
        protected void RemoveUserFromCache(string userId)
        {
            UserCache.RemoveUser(userId);
        }

        /// <summary>
        /// Ищет сведения о пользователе в локальном кэше.
        /// </summary>
        protected async Task<TUser> FindUserInCache(Func<TUser> cacheSelector, Func<Task<TUser>> storageSelector)
        {
            var user = cacheSelector();

            if (user == null)
            {
                user = await storageSelector();

                if (user != null)
                {
                    UserCache.AddOrUpdateUser(user);
                }
            }

            return user;
        }
    }
}