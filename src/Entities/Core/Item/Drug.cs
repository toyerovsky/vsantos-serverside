/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkInternals;
using Serverside.Core.Database.Models;

namespace Serverside.Entities.Core.Item
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