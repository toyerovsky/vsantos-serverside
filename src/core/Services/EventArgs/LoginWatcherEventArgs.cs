/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;

namespace VRP.Core.Services.EventArgs
{
    public class LoginWatcherEventArgs : System.EventArgs
    {
        public int AccountId { get; set; }
        // Temp token is assigned when user joins the server, it changes every join
        public Guid TempUserToken { get; set; }
    }
}