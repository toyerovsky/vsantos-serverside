/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VRP.Core.Tools;

namespace VRP.Core.Database.Forum
{
    public static class ForumDatabaseHelper
    {
        public static bool CheckPasswordMatch(string email, string password, out ForumUser forumUser)
        {
            forumUser = null;

            if (!email.Contains("@"))
                return false;


            using (IDbConnection connection =
                new MySqlConnection(Singletons.Configuration.GetConnectionString("forumConnectionString")))
            {
                var doesUserExistQuery = "SELECT COUNT(member_id) FROM core_members WHERE email = @email";
                int count = connection.QueryFirst<int>(doesUserExistQuery, new { email });

                if (count > 0)
                {
                    var query = "SELECT member_id as Id," +
                                " Name as UserName," +
                                " members_pass_hash as PasswordHash," +
                                " members_pass_salt as PasswordSalt," +
                                " member_group_id as GroupId," +
                                " mgroup_others as OtherGroups FROM core_members WHERE email = @email";

                    forumUser = connection.QueryFirst<ForumUser>(query, new { email });

                    string hash = GenerateIpbHash(password, forumUser.PasswordSalt);

                    return hash == forumUser.PasswordHash;
                }
            }

            return false;
        }

        private static string GenerateIpbHash(string plainText, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainText, "$2a$13$" + salt);
        }
    }
}
