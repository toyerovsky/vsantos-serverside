using System;

namespace VRP.BLL.Dto
{
    public class CharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool Online { get; set; }
        public float LastPositionX { get; set; }
        public float LastPositionY { get; set; }
        public float LastPositionZ { get; set; }
        public float LastRotationX { get; set; }
        public float LastRotationY { get; set; }
        public float LastRotationZ { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime BornDate { get; set; }
        public bool IsAlive { get; set; }
        public byte Health { get; set; }
        public bool Gender { get; set; }
        public decimal Money { get; set; }
        public decimal? BankMoney { get; set; }
        public AccountDto Account { get; set; }
        public int AccountId { get; set; }
        public string ImageUploadUrl { get; set; }
        public DateTime ImageUploadDate { get; set; }
    }
}