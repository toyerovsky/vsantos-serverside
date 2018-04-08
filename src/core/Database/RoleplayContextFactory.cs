/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VRP.Core.Tools;

namespace VRP.Core.Database
{
    public class RolePlayContextFactory : IDesignTimeDbContextFactory<RoleplayContext>
    {
        private static readonly RolePlayContextFactory Instance = new RolePlayContextFactory();

        public static RoleplayContext NewContext() => Instance.CreateDbContext(new string[] { });

        private bool _firstAttempt = true;
        public RoleplayContext CreateDbContext(string[] args)
        {
            if (_firstAttempt)
            {
                using (MySqlConnection testConnection = new MySqlConnection(Singletons.Configuration.GetConnectionString("gameConnectionString")))
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

                        Colorful.Console.WriteLine($"[{nameof(RolePlayContextFactory)}] Connected to database.", Color.Green);
                    }
                }
                _firstAttempt = false;
            }

            DbContextOptionsBuilder<RoleplayContext> options = new DbContextOptionsBuilder<RoleplayContext>();
            options.UseMySql(Singletons.Configuration.GetConnectionString("gameConnectionString"));
            
            return new RoleplayContext(options.Options);
        }
    }
}