/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using VRP.Core.Services.Model;

namespace VRP.Core.Services
{
    public interface IUsersWatcherService : IDisposable
    {
        event EventHandler<ActionData> AccountLoggedOut;
        event EventHandler<ActionData> AccountLoggedIn;
        event EventHandler ConnectionEstablished;
        void Watch();
    }
}