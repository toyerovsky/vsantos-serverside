/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Economy.Groups.Base;
using VRP.Serverside.Interfaces;

namespace VRP.Serverside.Entities.Core.Group
{
    public class GroupEntityFactory : IEntityFactory<GroupEntity, GroupModel>
    {
        public GroupEntity Create(GroupModel groupModel)
        {
            switch (groupModel.GroupType)
            {
                case GroupType.Crime: return new CrimeGroup(groupModel);
                case GroupType.CityHall: return new CityHall(groupModel);
                case GroupType.Police: return new Police(groupModel);
                default:
                    throw new NotSupportedException($"Not supported group type: {groupModel.GroupType}.");
            }
        }
    }
}
