/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Serverside.Core.Database
{
    public class RolePlayContextFactory : IDesignTimeDbContextFactory<RoleplayContext>
    {
        private static readonly RolePlayContextFactory Instance = new RolePlayContextFactory();

        public static RoleplayContext NewContext() => Instance.CreateDbContext(new string[] { });

        public RoleplayContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<RoleplayContext>();
            options.UseMySQL(new MySqlConnection(Constant.ServerInfo.Configuration.GetConnectionString("gameConnectionString")));
            return new RoleplayContext(options.Options);
        }
    }
}