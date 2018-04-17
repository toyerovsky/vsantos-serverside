namespace VRP.Core.Database.Models
{
    public class ZoneModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CreatorId { get; set; }
        public string ZonePropertiesJson { get; set; }
    }
}