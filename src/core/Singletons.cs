/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.Extensions.Configuration;
using VRP.Core.Database;
using VRP.Core.Tools;

namespace VRP.Core
{
    public static class Singletons
    {
        private static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .SetBasePath(Utils.WorkingDirectory)
            .AddJsonFile("coresettings.json")
            .Build();

        public static readonly RoleplayContextFactory RoleplayContextFactory =
            new RoleplayContextFactory(Configuration.GetConnectionString("GameConnectionString"));
    }
}