using System;

namespace VRP.Core.Services.LogOutBroadcaster
{
    public interface ILogOutBroadcasterService
    {
        void BroadcastLogOut(Guid userTempToken);
    }
}