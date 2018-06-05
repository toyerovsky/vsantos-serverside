/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using VRP.Core.Services.EventArgs;

namespace VRP.Core.Services.LogInWatcher
{
    public interface ILoginWatcherService
    {
        event EventHandler<LoginWatcherEventArgs> AccountLoggedIn;
    }
}