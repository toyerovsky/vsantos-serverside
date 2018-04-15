using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Admin
{
    public class AdminZoneManagerScript : Script
    {
        [Command("strefa")]
        public void OpenZoneManager(Client sender)
        {
            AccountEntity senderAccount = sender.GetAccountEntity();
            if (senderAccount.DbModel.ServerRank < ServerRank.AdministratorGry2)
            {
                sender.SendWarning("Nie posiadasz uprawnień do latania.");
                return;
            }

            sender.TriggerEvent(RemoteEvents.PlayerZoneManagerRequested);
        }
    }
}