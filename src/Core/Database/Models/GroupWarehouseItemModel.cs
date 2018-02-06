/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.ComponentModel.DataAnnotations;
using Serverside.Groups.Enums;

namespace Serverside.Core.Database.Models
{
    //Tabela do trzymania przedmiotów bazowych w magazynie
    public class GroupWarehouseItemModel
    {
        [Key]
        public long Id { get; set; }
        public decimal Cost { get; set; }
        public decimal? MinimalCost { get; set; }
        //Ile jest obecnie?
        public int Count { get; set; }
        //Ile nadaje się co tydzień?
        public int ResetCount { get; set; }

        public virtual ItemModel ItemModel { get; set; }
        [EnumDataType(typeof(GroupType))]
        public virtual GroupType GroupType { get; set; }
    }
}