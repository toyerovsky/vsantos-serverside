using System;

namespace VRP.vAPI.Dto
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
        public bool IsAlive { get; set; }
        public byte Health { get; set; }

        // foreign keys
        public int AccountId { get; set; }
    }
}