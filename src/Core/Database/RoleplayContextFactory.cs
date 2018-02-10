/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Serverside.Core.Database
{
    public class RolePlayContextFactory : IDesignTimeDbContextFactory<RoleplayContext>
    {
        private static readonly RolePlayContextFactory Instance = new RolePlayContextFactory();

        public static RoleplayContext NewContext() => Instance.CreateDbContext(new string[] { });

        public RoleplayContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<RoleplayContext>();
            options.UseMySql(/*Constant.ServerInfo.Configuration.GetConnectionString("gameConnectionString")*/"server=localhost;database=vsantossrv;Uid=root;Pwd=Dupa1234@;SslMode=none");
            
            return new RoleplayContext(options.Options);
        }
    }
}