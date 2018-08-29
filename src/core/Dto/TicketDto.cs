using System.Collections.Generic;
using VRP.DAL.Database.Models.Ticket;

namespace VRP.vAPI.Dto
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
   
        public string Type { get; set; }
        
        public string Status { get; set; }

        public virtual ICollection<TicketMessageDto> MessageContent { get; set; }
        public virtual ICollection<TicketUserRelation> InvolvedAccounts { get; set; }
        public virtual ICollection<TicketAdminRelation> InvolvedAdmins { get; set; }
    }
}