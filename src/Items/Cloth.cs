using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Entities.Core;

namespace Serverside.Items
{
    internal class Cloth : Item
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Cloth(EventClass events, ItemModel itemModel) : base(events, itemModel) { }

        public override void UseItem(AccountEntity player)
        {

        }

        public override string UseInfo => "Ten przedmiot zmienia ubranie Twojej postaci.";
    }
}