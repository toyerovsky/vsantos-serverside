/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Drawing;
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

        private bool _firstAttempt = true;
        public RoleplayContext CreateDbContext(string[] args)
        {
            if (_firstAttempt)
            {
                using (var testConnection = new MySqlConnection(Constant.ServerInfo.Configuration.GetConnectionString("gameConnectionString")))
                {
                    testConnection.Open();
                    var query = "select 1";

                    using (var command = new MySqlCommand(query, testConnection))
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

            var options = new DbContextOptionsBuilder<RoleplayContext>();
            options.UseMySql(Constant.ServerInfo.Configuration.GetConnectionString("gameConnectionString"));
            
            return new RoleplayContext(options.Options);
        }
    }
}