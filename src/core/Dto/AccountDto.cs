using System;
using System.Collections.Generic;

namespace VRP.BLL.Dto
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
        public bool UseGravatar { get; set; }
        public string PasswordSalt { get; set; }
        public ICollection<CharacterDto> Characters { get; set; }
    }
}
