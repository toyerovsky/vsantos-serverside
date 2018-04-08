/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Linq;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Serverside.Core.Scripts;

namespace VRP.Serverside.Entities.Core.Item
{
    internal class Mask : ItemEntity
    {
        public int UseCount => DbModel.FirstParameter.Value;

        /// <summary>
        /// Pierwszy parametr to liczba liczba użyć do zniszczenia
        /// </summary>
        /// <param name="itemModel"></param>
        public Mask(ItemModel itemModel) : base(itemModel) { }

        public override void UseItem(CharacterEntity character)
        {
            string encryptedName = $"Nieznajomy {character.FormatName.GetHashCode().ToString().Substring(1, 6)}";

            if (character.ItemsInUse.Any(item => ReferenceEquals(item, this)))
            {
                ChatScript.SendMessageToNearbyPlayers(character, "zdejmuje kominiarkę", ChatMessageType.Me);
                character.AccountEntity.Client.Name = character.FormatName;

                // FixMe zmienić na narzędzie do wyświetlania nicków
                // character.Client.ResetNametag();

                character.ItemsInUse.Remove(this);

                if (DbModel.FirstParameter.HasValue && DbModel.FirstParameter.Value == 0)
                {
                    Delete();
                    return;
                }
                Save();
            }
            else if (character.ItemsInUse.All(item => !(item is Mask)))
            {
                ChatScript.SendMessageToNearbyPlayers(character, "zakłada kominiarkę", ChatMessageType.Me);
                character.AccountEntity.Client.Name = encryptedName;

                // FixMe zmienić na narzędzie do wyświetlania nicków
                // character.Client.ResetNametag();

                DbModel.FirstParameter -= 1;
                character.ItemsInUse.Add(this);

                Save();
            }
        }

        public override string UseInfo => "Ten przedmiot ukrywa Twoją nazwę wyświetlaną.";
    }
}