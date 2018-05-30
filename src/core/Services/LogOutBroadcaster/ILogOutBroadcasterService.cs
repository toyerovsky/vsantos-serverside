/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;

namespace VRP.Core.Services.LogOutBroadcaster
{
    public interface ILogOutBroadcasterService
    {
        void BroadcastLogOut(Guid userTempToken);
    }
}