/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Serverside.Core.Database
{
    public class ForumDatabaseHelper
    {
        #region Nie patrzeć publicznie
        private readonly string _forumConnectionString = "server=v-santos.pl;uid=srv;pwd=WL8oTnufAAEFgoIt;database=vsantos;";
        #endregion

        public Tuple<long, string, short, string> CheckPasswordMatch(string email, string password)
        {
            if (UserExists(email))
            {
                using (MySqlConnection connection = new MySqlConnection(_forumConnectionString))
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
                                var othergroups = reader.GetString(5);
                                if (hash != "" && salt != "")
                                {

                                    if (hash.Equals(GenerateIpbHash(password, salt)))
                                    {
                                        return Tuple.Create(id, name, groupid, othergroups);
                                    }
                                    else
                                    {
                                        return Tuple.Create(-1L, "", short.Parse("-1"), "");
                                    }

                                }
                            }
                            return Tuple.Create(-1L, "", short.Parse("-1"), "");
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message + " | " + ex.StackTrace);
                        }
                        finally
                        {
                            reader.Close();
                            connection.Close();
                        }
                    }
                }
            }
            return Tuple.Create(-1L, "", short.Parse("-1"), "");
            //return 1; DEBUG !
        }

        public static string GenerateIpbHash(string plaintext, string salt)
        {
            //return CalculateMD5Hash(CalculateMD5Hash(salt) + CalculateMD5Hash(plaintext));
            return BCrypt.Net.BCrypt.HashPassword(plaintext, "$2a$13$" + salt);
            //return CreateMD5(CreateMD5(plaintext) + "$2a$13$" + salt);
        }

        public bool GetPassword(string login, out string password)
        {
            using (MySqlConnection connection = new MySqlConnection(_forumConnectionString))
            using (MySqlCommand command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT members_pass_hash FROM core_members WHERE email = @P0";

                command.Parameters.Add(new MySqlParameter("@P0", login));

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.Read())
                        {
                            password = reader.GetString(0);
                            return true;
                        }
                        password = string.Empty;
                        return false;
                    }
                    catch (Exception ex)
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

        public string GetSalt(string login)
        {
            using (MySqlConnection connection = new MySqlConnection(_forumConnectionString))
            using (MySqlCommand command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                command.CommandText = "SELECT members_pass_salt FROM core_members WHERE email = @P0";
                command.Parameters.Add(new MySqlParameter("@P0", login));

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    try
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                        return String.Empty;
                    }
                    catch (Exception ex)
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

        public string GetCustomField(long id)
        {
            using (MySqlConnection connection = new MySqlConnection(_forumConnectionString))
            using (MySqlCommand command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;

                command.CommandText = "SELECT field_2 FROM core_pfields_content WHERE member_id = @P0";
                command.Parameters.Add(new MySqlParameter("@P0", id));

                using (MySqlDataReader reader = command.ExecuteReader())
                {

                    try
                    {
                        if (reader.Read())
                        {
                            return reader.GetString(0);
                        }
                        return "";
                    }
                    catch (Exception ex)
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

        public bool UserExists(string login)
        {
            using (MySqlConnection connection = new MySqlConnection(_forumConnectionString))
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
                        if (reader.Read())
                        {
                            return reader.GetInt32(0) != 0;
                        }
                        return false;
                    }
                    catch (Exception ex)
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
