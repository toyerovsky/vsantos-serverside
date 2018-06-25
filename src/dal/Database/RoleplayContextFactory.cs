/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VRP.DAL.Database
{
    public class RoleplayContextFactory : IDesignTimeDbContextFactory<RoleplayContext>
    {
        public RoleplayContext Create() => this.CreateDbContext(new[] { "" });

        private readonly string _connectionString;

        // add this to auto generate migrations
        public RoleplayContextFactory() : this("server=77.55.212.185;database=vrpsrv;Uid=vrp;Pwd=kR6BNDBDNsX5yhJU;Convert Zero Datetime=True")
        {

        }

        public RoleplayContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public RoleplayContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<RoleplayContext> options = new DbContextOptionsBuilder<RoleplayContext>();
            options.UseMySql(_connectionString);
            return new RoleplayContext(options.Options);
        }
    }
}