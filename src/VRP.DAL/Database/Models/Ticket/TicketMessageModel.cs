using System;
using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Account;

namespace VRP.DAL.Database.Models.Ticket
{
    public class TicketMessageModel
    {
        public int Id { get; set; }
        public string MessageContent { get; set; }
        public DateTime CreationTime { get; set; }

        // foreign keys
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }

        // navigation properties
        public virtual AccountModel Author { get; set; }
        public virtual TicketModel Ticket { get; set; }
    }
}

