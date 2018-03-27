using VRP.Core.Enums;

namespace VRP.vAPI.Services.Model
{
    public class UserData
    {
        public int AccountId { get; set; }
        public int CharacterId { get; set; }
        public string Token { get; set; }
        public BroadcasterActionType ActionType { get; set; }
    }
}