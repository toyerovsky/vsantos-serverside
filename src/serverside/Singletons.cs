/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.Extensions.Configuration;
using VRP.Core.Interfaces;
using VRP.Core.Services;
using VRP.Core.Tools;

namespace VRP.Serverside
{
    public static class Singletons
    {
        public static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Utils.WorkingDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        private static readonly ILogger Logger = new ConsoleLogger();
        public static readonly IUserBroadcasterService UserBroadcasterService = new UsersBroadcasterService(Configuration, Logger);
        public static readonly IUsersWatcherService UsersWatcherService = new UsersWatcherService(Configuration, Logger);
    }
}