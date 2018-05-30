/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

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