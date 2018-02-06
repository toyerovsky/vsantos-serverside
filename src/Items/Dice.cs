using System;
using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Scripts;
using Serverside.Entities.Core;

namespace Serverside.Items
{
    internal class Dice : Item
    {
        /// <summary>
        /// Pierwszy parametr to liczba oczek na kostce
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Dice(EventClass events, ItemModel itemModel) : base(events, itemModel) { }

        public override void UseItem(AccountEntity player)
        {
            if (DbModel.FirstParameter != null)
                ChatScript.SendMessageToNearbyPlayers(player.Client,
                    $"wyrzucił {new Random().Next(1, DbModel.FirstParameter.Value)} oczek z {DbModel.FirstParameter} możliwych",
                    ChatMessageType.ServerMe);
        }

        public override string UseInfo => $"Ten przedmiot zwraca losową liczbę od 1 do {DbModel.FirstParameter}";
    }
}