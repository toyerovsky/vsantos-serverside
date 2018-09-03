using System.Collections.Generic;
using VRP.DAL.Database.Models.Ticket;

namespace VRP.BLL.Dto
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
   
        public string Type { get; set; }
        
        public string Status { get; set; }

        public virtual IEnumerable<TicketMessageDto> MessageContent { get; set; }
        public virtual IEnumerable<TicketUserRelation> InvolvedAccounts { get; set; }
        public virtual IEnumerable<TicketAdminRelation> InvolvedAdmins { get; set; }
    }
}