/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using Newtonsoft.Json;
using VRP.Core.Services.EventArgs;

namespace VRP.Core.Services.LogInWatcher
{
    /// <summary>
    /// UsersWatcher watches for user login and log out
    /// </summary>
    public class LoginWatcherService : ILoginWatcherService
    {
        public event EventHandler<LoginWatcherEventArgs> AccountLoggedIn;

        public LoginWatcherService(IWatcher watcher)
        {
            watcher.DataRecieved += OnDataRecieved;
        }

        private void OnDataRecieved(object sender, DataRecievedEventArgs dataRecievedEventArgs)
        {
            if (dataRecievedEventArgs.Header == "LogIn")
            {
                LoginWatcherEventArgs e =
                    JsonConvert.DeserializeObject<LoginWatcherEventArgs>(dataRecievedEventArgs.Json);

                AccountLoggedIn?.Invoke(this, e);
            }
        }
    }
}