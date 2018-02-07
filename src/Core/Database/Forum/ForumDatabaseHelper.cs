/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Data;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Serverside.Core.Database.Forum
{
    public class ForumDatabaseHelper
    {
        public bool CheckPasswordMatch(string email, string password, out ForumLoginData forumLoginData)
        {
            forumLoginData = null;

            if (UserExists(email))
            {
                using (MySqlConnection connection = new MySqlConnection(Constant.ServerInfo.Configuration.GetConnectionString("forumConnectionString")))
                using (MySqlCommand command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "SELECT member_id, Name, members_pass_hash, members_pass_salt, member_group_id, mgroup_others FROM core_members WHERE email = @P0";

                    command.Parameters.Add(new MySqlParameter("@P0", email));

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        try
                        {
                            if (reader.Read())
                            {
                                var id = reader.GetInt64(0);
                                var name = reader.GetString(1);
                                var hash = reader.GetString(2);
                                var salt = reader.GetString(3);
                                var groupid = reader.GetInt16(4);
                                var otherGroups = reader.GetString(5);

                                if (hash != "" && salt != "")
                                {
                                    if (hash.Equals(GenerateIpbHash(password, salt)))
                                        forumLoginData = new ForumLoginData(id, name, groupid, otherGroups);
                                }
                            }
                        }
                        catch (MySqlException ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        finally
                        {
                            reader.Close();
                            connection.Close();
                        }
                    }
                }
            }

            return forumLoginData != null;
        }

        public static string GenerateIpbHash(string plaintext, string salt)
        {
            //return CalculateMD5Hash(CalculateMD5Hash(salt) + CalculateMD5Hash(plaintext));
            return BCrypt.Net.BCrypt.HashPassword(plaintext, "$2a$13$" + salt);
            //return CreateMD5(CreateMD5(plaintext) + "$2a$13$" + salt);
        }

        public bool UserExists(string login)
        {
            using (MySqlConnection connection = new MySqlConnection(Constant.ServerInfo.Configuration.GetConnectionString("forumConnectionString")))
            using (MySqlCommand command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                command.CommandText =
                    "SELECT COUNT(member_id) FROM core_members WHERE email = @P0";
                command.Parameters.Add(new MySqlParameter("@P0", login));

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        return reader.Read() && reader.GetInt32(0) != 0;
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        reader.Close();
                        connection.Close();
                    }
                }
            }
        }
    }
}
