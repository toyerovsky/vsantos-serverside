using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Bot;
using VRP.DAL.Database.Models.Group;

namespace VRP.BLL.Dto
{
    public class CrimeBotDto
    {
        public int Id { get; set; }
        public string VehicleModel { get; set; }
        public int CreatorId { get; set; }
        public int GroupModelId { get; set; }
        public int BotModelId { get; set; }

    }
}
