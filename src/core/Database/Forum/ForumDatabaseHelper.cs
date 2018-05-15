/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace VRP.Core.Database.Forum
{
    public class ForumDatabaseHelper
    {
        private readonly IConfiguration _configuration;

        public ForumDatabaseHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool CheckPasswordMatch(string email, string password, out ForumUser forumUser)
        {
            forumUser = null;

            if (!email.Contains("@"))
                return false;

            using (IDbConnection connection =
                new MySqlConnection(_configuration.GetConnectionString("forumConnectionString")))
            {
                var query = "SELECT member_id as Id," +
                            " Name as UserName," +
                            " members_pass_hash as PasswordHash," +
                            " members_pass_salt as PasswordSalt," +
                            " member_group_id as GroupId," +
                            " mgroup_others as OtherGroups FROM core_members WHERE email = @email";

                forumUser = connection.QueryFirstOrDefault<ForumUser>(query, new { email });

                if (forumUser != null)
                {
                    string hash = GenerateIpbHash(password, forumUser.PasswordSalt);
                    return hash == forumUser.PasswordHash;
                }

                return false;
            }
        }

        private string GenerateIpbHash(string plainText, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainText, "$2a$13$" + salt);
        }
    }
}
