using System;
using VRP.Core.Services.EventArgs;
using VRP.Core.Services.Model;

namespace VRP.Core.Services.LogOutWatcher
{
    public interface ILogOutWatcherService
    {
        event EventHandler<LogOutWatcherEventArgs> AccountLoggedOut;
    }
}