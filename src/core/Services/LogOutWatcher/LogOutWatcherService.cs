using System;
using Newtonsoft.Json;
using VRP.Core.Services.EventArgs;

namespace VRP.Core.Services.LogOutWatcher
{
    public class LogOutWatcherService : ILogOutWatcherService
    {
        public event EventHandler<LogOutWatcherEventArgs> AccountLoggedOut;

        public LogOutWatcherService(IWatcher watcher)
        {
            watcher.DataRecieved += OnDataRecieved;
        }

        private void OnDataRecieved(object sender, DataRecievedEventArgs dataRecievedEventArgs)
        {
            if (dataRecievedEventArgs.Header == "LogOut")
            {
                LogOutWatcherEventArgs e =
                    JsonConvert.DeserializeObject<LogOutWatcherEventArgs>(dataRecievedEventArgs.Json);

                AccountLoggedOut?.Invoke(this, e);
            }
        }
    }
}