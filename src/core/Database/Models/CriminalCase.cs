

namespace VRP.Core.Database.Models
{
    public class CriminalCase
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Record> Involved { get; set; }
    }
}