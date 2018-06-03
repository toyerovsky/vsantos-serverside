using System;
using System.Collections.Generic;
using System.Text;

namespace VRP.Core.Database.Models
{
    public class CrimeBotItemModel
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public int Count { get; set; }
        public int ResetCount { get; set; }
        public virtual ItemTemplateModel ItemTemplateModel { get; set; }
    }
}
