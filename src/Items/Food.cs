using System;
using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Scripts;
using Serverside.Entities.Core;

namespace Serverside.Items
{
    internal class Food : Item
    {
        /// <summary>
        /// Pierwszy parametr to ilość HP do przyznania
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Food(EventClass events, ItemModel itemModel) : base(events, itemModel)
        {
        }

        public override void UseItem(AccountEntity player)
        {
            ChatScript.SendMessageToNearbyPlayers(player.Client, $"zjada {DbModel.Name}", ChatMessageType.ServerMe);
            player.Client.Health += Convert.ToInt32(DbModel.FirstParameter);
            Delete();
        }

        public override string UseInfo => $"Ten przedmiot odnawia: {DbModel.FirstParameter} punktów życia.";
    }
}