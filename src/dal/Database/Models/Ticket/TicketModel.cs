using System.Collections.Generic;


namespace VRP.Database.Models.Ticket
{
    public class TicketModel
    {
        public InformationContainer()
        {
            MessageContent = new HashSet<TicketMessageModel>();
            InvolvedAccounts = new HashSet<AccountModel>();
            InvolvvedAdmins = new HashSet<AccountModel>();
        }
        
        
        public int Id { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public bool Closed { get; set; }

        public virtual ICollection<TicketMessageModel> MessageContent { get; set; }
        public virtual ICollection<AccountModel> InvolvedAccounts { get; set; }
        public virtual ICollection<AccountModel> InvolvedAdmins { get; set; }


    }
}

