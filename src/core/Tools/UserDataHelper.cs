using VRP.DAL.Database.Forum;

namespace VRP.Core.Tools
{
    public static class UserDataHelper
    {
        public static bool CheckPasswordMatch(ForumUser forumUser, string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword, "$2a$13$" + forumUser.PasswordSalt) == forumUser.PasswordHash;
        }
    }
}