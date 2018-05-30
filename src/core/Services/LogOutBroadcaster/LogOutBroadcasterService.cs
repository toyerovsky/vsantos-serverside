/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using Newtonsoft.Json;
using VRP.Core.Services.EventArgs;

namespace VRP.Core.Services.LogOutBroadcaster
{
    public class LogOutBroadcasterService : ILogOutBroadcasterService
    {
        private readonly IBroadcaster _broadcaster;

        public LogOutBroadcasterService(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        public void BroadcastLogOut(Guid userTempToken)
        {
            DataRecievedEventArgs e = new DataRecievedEventArgs
            {
                Header = "LogIn",
                Json = JsonConvert.SerializeObject(new { userTempToken })
            };

            _broadcaster.Broadcast(JsonConvert.SerializeObject(e));
        }

        public void Dispose()
        {
            _broadcaster?.Dispose();
        }
    }
}