/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

namespace VRP.Core.Database.Forum
{
    public class ForumUser
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public short GroupId { get; set; }
        public string OtherGroups { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}