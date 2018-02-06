using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Entities.Core;

namespace Serverside.Items
{
    internal class WeaponClip : Item
    {
        /// <summary>
        /// Pierwszy parametr to hash broni do której pasuje, a drugi to ilość amunicji
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public WeaponClip(EventClass events, ItemModel itemModel) : base(events, itemModel) { }

        public override void UseItem(AccountEntity player)
        {
        }

        public override string UseInfo => $"Ten przedmiot dodaje {DbModel.SecondParameter} naboi do broni.";
    }
}