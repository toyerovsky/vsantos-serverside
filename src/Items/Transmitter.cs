using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Entities.Core;

namespace Serverside.Items
{
    internal class Transmitter : Item
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Transmitter(EventClass events, ItemModel itemModel) : base(events, itemModel) { }

        public override void UseItem(AccountEntity player)
        {
        }

        public override string UseInfo => $"Przedmiot służy do komunikacji na podanej częstotliwości w zasięgu: {DbModel.SecondParameter}";
    }
}