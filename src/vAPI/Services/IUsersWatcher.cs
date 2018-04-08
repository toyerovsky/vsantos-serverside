﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using VRP.vAPI.Model;

namespace VRP.vAPI.Services
{
    public interface IUsersWatcher
    {
        bool IsUserOnline(Guid token);
        bool TryGetUser(Guid token, out AppUser appUser);
    }
}