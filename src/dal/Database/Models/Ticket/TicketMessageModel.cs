using System;
using System.Collections.Generic;


namespace VRP.Database.Models.Ticket
{
    public class TicketMessageModel
    {
        public InformationContainer()
        {
            InvolvedAccounts = new HashSet<AccountModel>();
        }

        public int Id { get; set; }
        public string ContentMessage { get; set; }
        public DateTime CreationTime { get; set; }

        public virtual ICollection<AccountModel> InvolvedAccounts { get; set; }

    }
}

