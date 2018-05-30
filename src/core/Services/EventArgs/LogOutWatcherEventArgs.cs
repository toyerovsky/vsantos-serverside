using System;

namespace VRP.Core.Services.EventArgs
{
    public class LogOutWatcherEventArgs : System.EventArgs
    {
        public Guid TempUserToken { get; set; }
    }
}