/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.ComponentModel.DataAnnotations;
using Serverside.Entities.Core.Item;

namespace Serverside.Core.Database.Models
{
    public class ItemModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual CharacterModel Character { get; set; }
        public virtual BuildingModel Building { get; set; }
        public virtual VehicleModel Vehicle { get; set; }
        public virtual GroupModel Group { get; set; }

        public virtual AccountModel Creator { get; set; }

        public int Weight { get; set; }
        public string ItemHash { get; set; }

        public int? FirstParameter { get; set; }
        public int? SecondParameter { get; set; }
        public int? ThirdParameter { get; set; }
        public int? FourthParameter { get; set; }

        [EnumDataType(typeof(ItemType))]
        public virtual ItemType ItemType { get; set; }
    }
}
