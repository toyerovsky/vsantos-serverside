/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Net;
using Microsoft.Extensions.Configuration;
using VRP.Core.Interfaces;
using VRP.Core.Services;
using VRP.Core.Services.LogInWatcher;
using VRP.Core.Tools;

namespace VRP.Serverside
{
    public static class Singletons
    {
        public static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Utils.WorkingDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        public static readonly ILogger Logger = new ConsoleLogger();
        public static readonly IWatcher Watcher = new SocketWatcher(IPAddress.Loopback, 2137, Logger);
        public static readonly ILoginWatcherService LogInWatcher = new LoginWatcherService(Watcher);
    }
}