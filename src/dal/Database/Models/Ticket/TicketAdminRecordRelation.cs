using VRP.DAL.Database.Models.Account;

namespace VRP.DAL.Database.Models.Ticket
{
    public class TicketAdminRecordRelation
    {
        public int Id { get; set; }

        public virtual TicketModel Ticket { get; set; }
        public virtual AccountModel Admin { get; set; }
    }
}