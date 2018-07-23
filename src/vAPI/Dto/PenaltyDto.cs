using System;

namespace VRP.vAPI.Dto
{
    public class PenaltyDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string PenaltyType { get; set; }
        public string Reason { get; set; }
        public AccountDto Creator { get; set; }
    }
}