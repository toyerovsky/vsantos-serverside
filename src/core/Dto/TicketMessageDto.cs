namespace VRP.vAPI.Dto
{
    public class TicketMessageDto
    {
        public int Id { get; set; }
        public string MessageContent { get; set; }
        public virtual AccountDto Author { get; set; }
        public virtual TicketDto Ticket { get; set; }
    }
}