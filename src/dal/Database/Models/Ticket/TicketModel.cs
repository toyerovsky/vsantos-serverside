using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Enums;

namespace VRP.DAL.Database.Models.Ticket
{
    public class TicketModel
    {
        public TicketModel()
        {
            MessageContent = new HashSet<TicketMessageModel>();
            InvolvedAccounts = new HashSet<TicketAdminRelation>();
            InvolvedAdmins = new HashSet<TicketUserRelation>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        [EnumDataType(typeof(TicketType))]
        public TicketType Type { get; set; }
        [EnumDataType(typeof(TicketStatusType))]
        public TicketStatusType Status { get; set; }

        public virtual ICollection<TicketMessageModel> MessageContent { get; set; }
        public virtual ICollection<TicketAdminRelation> InvolvedAccounts { get; set; }
        public virtual ICollection<TicketUserRelation> InvolvedAdmins { get; set; }
    }
}

