﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using Serverside.Core.Database.Models;

namespace Serverside.Entities.Core.Item
{
    internal class Cloth : Item
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemModel"></param>
        public Cloth(ItemModel itemModel) : base(itemModel) { }

        public override void UseItem(AccountEntity player)
        {

        }

        public override string UseInfo => "Ten przedmiot zmienia ubranie Twojej postaci.";
    }
}