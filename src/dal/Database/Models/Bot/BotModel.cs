namespace VRP.DAL.Database.Models.Bot
{
    public class BotModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public uint PedHash { get; set; }
        public int CreatorId { get; set; }
    }
}