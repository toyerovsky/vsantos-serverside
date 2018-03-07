﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using Serverside.Core.Database.Models;

namespace Serverside.Entities.Core.Item
{
    internal class Drug : ItemEntity
    {
        /// <summary>
        /// Pierwszy parametr to drugtype
        /// </summary>
        /// <param name="itemModel"></param>
        public Drug(ItemModel itemModel) : base(itemModel) { }
    }
}