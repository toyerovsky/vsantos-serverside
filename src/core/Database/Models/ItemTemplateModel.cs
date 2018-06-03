using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models
{
    public class ItemTemplateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public short Weight { get; set; }
        public string ItemHash { get; set; }

        public int? FirstParameter { get; set; }
        public int? SecondParameter { get; set; }
        public int? ThirdParameter { get; set; }
        public int? FourthParameter { get; set; }

        [EnumDataType(typeof(ItemEntityType))]
        public virtual ItemEntityType ItemEntityType { get; set; }
    }
}
