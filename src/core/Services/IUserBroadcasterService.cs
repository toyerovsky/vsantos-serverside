using System;
using VRP.Core.Enums;

namespace VRP.Core.Interfaces
{
    public interface IUserBroadcasterService : IDisposable
    {
        void Broadcast(int accountId, int characterId, Guid token, BroadcasterActionType actionType);
        void Prepare();
    }
}