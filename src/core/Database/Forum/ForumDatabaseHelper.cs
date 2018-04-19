/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace VRP.Core.Database.Forum
{
    public class ForumDatabaseHelper
    {
        private string _connectionString =
            "server=77.55.212.185;database=vrpforum;uid=vrp;pwd=kR6BNDBDNsX5yhJU;SslMode=None";

        public bool CheckPasswordMatch(string email, string password, out ForumLoginData forumLoginData)
        {
            forumLoginData = null;
            if (UserExists(email))
            {
                using (MySqlConnection connection = new MySqlConnection(_connectionString/*Singletons.Configuration.GetConnectionString("forumConnectionString")*/))
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
                                long id = reader.GetInt64(0);
                                string name = reader.GetString(1);
                                string hash = reader.GetString(2);
                                string salt = reader.GetString(3);
                                short groupId = reader.GetInt16(4);
                                string otherGroups = reader.GetString(5);

                                if (hash != "" && salt != "")
                                {
                                    if (hash.Equals(GenerateIpbHash(password, salt)))
                                        forumLoginData = new ForumLoginData(id, name, groupId, otherGroups);
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
            using (MySqlConnection connection = new MySqlConnection(/*Singletons.Configuration.GetConnectionString("forumConnectionString")*/_connectionString))
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
