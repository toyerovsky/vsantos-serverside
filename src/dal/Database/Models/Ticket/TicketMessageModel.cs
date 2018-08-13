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
        [ForeignKey("TicketAuthor")]
        public int AuthorId { get; set; }
        public virtual AccountModel Author { get; set; }
        [ForeignKey("Ticket")]
        public int TicketId { get; set; }
        public virtual TicketModel Ticket { get; set; }

    }
}

