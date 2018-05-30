using System;

namespace VRP.Core.Services.LogInBroadcaster
{
    public interface ILogInBroadcasterService
    {
        void BroadcastLogin(int accountId, int characterId, Guid userTempToken);
    }
}