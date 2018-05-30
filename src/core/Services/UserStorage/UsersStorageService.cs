/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using VRP.Core.Services.Model;

namespace VRP.Core.Services.UserStorage
{
    public class UsersStorageService : IUsersStorageService
    {
        /// <summary>
        /// Key is user token
        /// </summary>
        private readonly Dictionary<Guid, AppUser> _onlineUsers = new Dictionary<Guid, AppUser>();

        public bool IsUserOnline(string email)
        {
            return _onlineUsers.Any(user => user.Value.Email == email);
        }

        public bool IsUserOnline(Guid token) => _onlineUsers.ContainsKey(token);

        public bool TryGetUser(Guid token, out AppUser appUser) =>
            _onlineUsers.TryGetValue(token, out appUser);

        public void Login(Guid userGuid, int accountId)
        {
            AppUser appUser = new AppUser
            {
                UserAccountId = accountId,
            };

            _onlineUsers.Add(userGuid, appUser);
        }

        public bool TrySelectCharacter(Guid userId, int characterId)
        {
            if (_onlineUsers.TryGetValue(userId, out AppUser appUser) && appUser.SelectedCharacterId == -1)
                appUser.SelectedCharacterId = characterId;

            // account already has a selected character or
            // account is not logged
            return appUser?.SelectedCharacterId != null;
        }

        public void LogOut(Guid token)
        {
            _onlineUsers.Remove(token);
        }
    }
}
