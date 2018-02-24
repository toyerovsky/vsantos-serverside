/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Linq;
using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Scripts;

namespace Serverside.Entities.Core.Item
{
    internal class Mask : Item
    {
        /// <summary>
        /// Pierwszy parametr to liczba liczba użyć do zniszczenia
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Mask(EventClass events, ItemModel itemModel) : base(events, itemModel) { }

        public override void UseItem(AccountEntity player)
        {
            var encryptedName = $"Nieznajomy {player.Client.Name.GetHashCode().ToString().Substring(1, 6)}";
            
            if (player.CharacterEntity.ItemsInUse.Any(item => ReferenceEquals(item, this)))
            {
                ChatScript.SendMessageToNearbyPlayers(player.Client, "zdejmuje kominiarkę", ChatMessageType.Me);

                player.Client.Name = player.CharacterEntity.FormatName;
                player.Client.ResetNametag();

                player.CharacterEntity.ItemsInUse.Remove(this);

                if (DbModel.FirstParameter.HasValue && DbModel.FirstParameter.Value == 0)
                {
                    Delete();
                    return;
                }
                Save();
            }
            else if (player.CharacterEntity.ItemsInUse.All(item => !(item is Mask)))
            {
                ChatScript.SendMessageToNearbyPlayers(player.Client, "zakłada kominiarkę", ChatMessageType.Me);
                player.Client.Name = encryptedName;
                player.Client.ResetNametag();

                DbModel.FirstParameter -= 1;
                player.CharacterEntity.ItemsInUse.Add(this);

                Save();
            }
        }

        public override string UseInfo => "Ten przedmiot ukrywa Twoją nazwę wyświetlaną.";
    }
}