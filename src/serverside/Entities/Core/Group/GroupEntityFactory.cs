using System;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Entities.Interfaces;
using VRP.Serverside.Economy.Groups.Base;

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
