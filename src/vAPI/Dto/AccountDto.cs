using System;

namespace VRP.vAPI.Dto
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string ForumUserName { get; set; }
        public string Email { get; set; }
        public string ServerRank { get; set; }
        public DateTime LastLogin { get; set; }
        public string AvatarUrl { get; set; }
        public string GravatarEmail { get; set; }
        public string PasswordSalt { get; set; }
    }
}
