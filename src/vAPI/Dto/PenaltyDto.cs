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
        public int CreatorId { get; set; }
        public AccountDto Creator { get; set; }
        public int CharacterId { get; set; }
        public CharacterDto Character { get; set; }
        public int AccountId { get; set; }
        public AccountDto Account { get; set; }
        public int DeactivatorId { get; set; }
        public AccountDto Deactivator { get; set; }
    }
}