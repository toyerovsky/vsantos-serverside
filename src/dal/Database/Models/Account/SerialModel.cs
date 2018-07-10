using System.ComponentModel.DataAnnotations.Schema;

namespace VRP.DAL.Database.Models.Account
{
    public class SerialModel
    {
        public int Id { get; set; }
        public string GameSerial { get; set; }

        // navigation properties
        [ForeignKey("AccountModel")]
        public int AccountId { get; set; }
        public virtual AccountModel AccountModel { get; set; }
    }
}