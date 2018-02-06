using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Scripts;
using Serverside.Entities.Core;

namespace Serverside.Items
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

            if (DbModel.CurrentlyInUse)
            {
                ChatScript.SendMessageToNearbyPlayers(player.Client, "zdejmuje kominiarkę", ChatMessageType.Me);

                player.Client.Name = player.CharacterEntity.FormatName;
                player.Client.ResetNametag();

                DbModel.CurrentlyInUse = false;
                if (DbModel.FirstParameter.HasValue && DbModel.FirstParameter.Value == 0)
                {
                    Delete();
                    return;
                }
                Save();
            }
            else
            {
                ChatScript.SendMessageToNearbyPlayers(player.Client, "zakłada kominiarkę", ChatMessageType.Me);
                player.Client.Name = encryptedName;
                player.Client.ResetNametag();

                DbModel.FirstParameter -= 1;
                DbModel.CurrentlyInUse = true;

                Save();
            }
        }

        public override string UseInfo => "Ten przedmiot ukrywa Twoją nazwę wyświetlaną.";
    }
}