/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace VRP.DAL.Database.Forum
{
    public class ForumDatabaseHelper
    {
        private readonly string _connectionString;

        public ForumDatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ForumUser GetForumUserByEmail(string email)
        {
            if (!email.Contains("@"))
                return null;

            using (IDbConnection connection =
                new MySqlConnection(_connectionString))
            {
                var query = "SELECT member_id as Id," +
                            " Name as UserName," +
                            " members_pass_hash as PasswordHash," +
                            " members_pass_salt as PasswordSalt," +
                            " member_group_id as GroupId," +
                            " email as Email," +
                            " mgroup_others as OtherGroups FROM core_members WHERE email = @email";

                ForumUser forumUser = connection.QueryFirstOrDefault<ForumUser>(query, new { email });
                return forumUser;
            }
        }
    }
}
