/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MySql.Data.MySqlClient;

namespace VRP.Core.Database
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

        private bool _firstAttempt = true;
        public RoleplayContext CreateDbContext(string[] args)
        {
            if (_firstAttempt)
            {
                using (MySqlConnection testConnection = new MySqlConnection(_connectionString))
                {
                    testConnection.Open();
                    string query = "select 1";

                    using (MySqlCommand command = new MySqlCommand(query, testConnection))
                    {
                        try
                        {
                            command.ExecuteScalar();
                        }
                        catch (Exception e)
                        {
                            Colorful.Console.WriteLine("Database error occured: ", Color.Red);
                            Colorful.Console.WriteLine(e, Color.DarkRed);
                            throw;
                        }

                        Colorful.Console.WriteLine($"[{nameof(RoleplayContextFactory)}] Connected to database.", Color.Green);
                    }
                }
                _firstAttempt = false;
            }

            DbContextOptionsBuilder<RoleplayContext> options = new DbContextOptionsBuilder<RoleplayContext>();
            options.UseMySql(_connectionString);

            return new RoleplayContext(options.Options);
        }
    }
}