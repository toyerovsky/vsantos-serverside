using System;
using Serverside.Core.Database.Models;
using Serverside.Entities.Interfaces;
using Serverside.Economy.Groups.Enums;
using Serverside.Economy.Groups.Base;

namespace Serverside.Entities.Core.Group
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
