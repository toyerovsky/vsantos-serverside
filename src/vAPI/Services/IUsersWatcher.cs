using System;
using VRP.vAPI.Model;

namespace VRP.vAPI.Services
{
    public interface IUsersWatcher
    {
        bool IsUserOnline(Guid token);
        bool TryGetUser(Guid token, out AppUser appUser);
    }
}