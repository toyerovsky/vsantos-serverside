/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.Extensions.Configuration;

namespace VRP.Core.Tools
{
    public static class Singletons
    {
        public static readonly UserBroadcaster UserBroadcaster = new UserBroadcaster();

        public static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Utils.WorkingDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
    }
}