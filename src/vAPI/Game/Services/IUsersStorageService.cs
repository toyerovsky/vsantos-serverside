using System;
using VRP.Core.Database.Models;
using VRP.vAPI.Game.Model;

namespace VRP.vAPI.Game.Services
{
    public interface IUsersStorageService
    {
        bool IsUserOnline(string email);
        bool IsUserOnline(Guid token);
        bool TryGetUser(Guid token, out AppUser appUser);
        void Login(Guid userGuid, AccountModel accountModel);
        /// <summary>
        /// Method returns true if character selection ended successfully or false if not
        /// </summary>
        /// <param name="characterModel"></param>
        /// <returns></returns>
        bool SelectCharacter(CharacterModel characterModel);
        void LogOut(Guid token);
    }
}