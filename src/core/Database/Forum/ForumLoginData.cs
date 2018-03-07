/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

namespace Serverside.Core.Database.Forum
{
    public class ForumLoginData
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public short GroupId { get; set; }
        public string OtherGroups { get; set; }

        public ForumLoginData(long id, string userName, short groupId, string otherGroups)
        {
            Id = id;
            UserName = userName;
            GroupId = groupId;
            OtherGroups = otherGroups;
        }
    }
}