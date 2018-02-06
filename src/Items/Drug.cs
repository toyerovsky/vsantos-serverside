using GTANetworkInternals;
using Serverside.Core.Database.Models;

namespace Serverside.Items
{
    internal class Drug : Item
    {
        /// <summary>
        /// Pierwszy parametr to drugtype
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Drug(EventClass events, ItemModel itemModel) : base(events, itemModel) { }
    }
}