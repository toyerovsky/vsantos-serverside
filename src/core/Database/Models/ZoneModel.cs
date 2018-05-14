using System.ComponentModel.DataAnnotations;
using VRP.Core.Enums;

namespace VRP.Core.Database.Models
{
    public class ZoneModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        [EnumDataType(typeof(ZoneType))]
        public virtual ZoneType ZoneType { get; set; }
        public string ZonePropertiesJson { get; set; }
    }
}