using System;

namespace VRP.BLL.Dto
{
    public class ItemTemplateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ItemType { get; set; }
        public int? FirstParameter { get; set; }
        public int? SecondParameter { get; set; }
        public int? ThirdParameter { get; set; }
        public int? FourthParameter { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}