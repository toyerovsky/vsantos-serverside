using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VRP.Core.Database.Models;
using VRP.vAPI.Game.Model;

namespace VRP.vAPI.Game.Services
{
    public class UsersStorageService : IUsersStorageService
    {
        /// <summary>
        /// Key is user token
        /// </summary>
        private readonly Dictionary<Guid, AppUser> _onlineUsers = new Dictionary<Guid, AppUser>();

        public bool IsUserOnline(string email)
        {
            return _onlineUsers.Any(user => user.Value.UserAccount.Email == email);
        }

        public bool IsUserOnline(Guid token) => _onlineUsers.ContainsKey(token);

        public bool TryGetUser(Guid token, out AppUser appUser) =>
            _onlineUsers.TryGetValue(token, out appUser);

        public void Login(Guid userGuid, AccountModel accountModel)
        {
            AppUser appUser = new AppUser
            {
                UserAccount = accountModel,
            };

            _onlineUsers.Add(userGuid, appUser);
        }

        public bool SelectCharacter(CharacterModel characterModel)
        {
            AppUser appUser =
                _onlineUsers.SingleOrDefault(user => user.Value.UserAccount.Id == characterModel.Account.Id).Value;

            if (appUser != null && appUser.SelectedCharacter == null)
                appUser.SelectedCharacter = characterModel;

            // account already has a selected character or
            // account is not logged
            return appUser?.SelectedCharacter != null;
        }

        public void LogOut(Guid token)
        {
            _onlineUsers.Remove(token);
        }
    }
}
