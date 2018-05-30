/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using Newtonsoft.Json;
using VRP.Core.Services.EventArgs;

namespace VRP.Core.Services.LogInBroadcaster
{
    public class LogInBroadcasterService : ILogInBroadcasterService
    {
        private readonly IBroadcaster _broadcaster;

        public LogInBroadcasterService(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public void BroadcastLogin(int accountId, int characterId, Guid userTempToken)
        {
            DataRecievedEventArgs e = new DataRecievedEventArgs
            {
                Header = "LogIn",
                Json = JsonConvert.SerializeObject(
                    new
                    {
                        accountId,
                        characterId,
                        userTempToken
                    })
            };

            _broadcaster.Broadcast(JsonConvert.SerializeObject(e));
        }

        public void Dispose()
        {
            _broadcaster?.Dispose();
        }
    }
}