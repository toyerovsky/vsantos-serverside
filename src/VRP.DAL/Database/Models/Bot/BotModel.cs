using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Account;

namespace VRP.DAL.Database.Models.Bot
{
    public class BotModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public uint PedHash { get; set; }

        // foreign keys
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }

        // navigation properties
        public virtual AccountModel Creator { get; set; }
    }
}