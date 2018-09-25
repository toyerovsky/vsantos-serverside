using System;
using System.Collections.Generic;
using System.Text;


namespace VRP.BLL.Dto
{
    public class CrimeBotItemDto
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; }
        public int ResetCount { get; set; }
        public int ItemTemplateModelId { get; set; }
        public ItemTemplateDto ItemTemplate { get; set; }
    }
}
