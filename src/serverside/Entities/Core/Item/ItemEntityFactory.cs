/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Enums;
using VRP.Serverside.Interfaces;

namespace VRP.Serverside.Entities.Core.Item
{
    public class ItemEntityFactory : IEntityFactory<ItemEntity, ItemModel>
    {
        public ItemEntity Create(ItemModel itemModel)
        {
            switch (itemModel.ItemEntityType)
            {
                case ItemEntityType.Food: return new Food(itemModel);
                case ItemEntityType.Weapon: return new Weapon(itemModel);
                case ItemEntityType.WeaponClip: return new WeaponClip(itemModel);
                case ItemEntityType.Mask: return new Mask(itemModel);
                case ItemEntityType.Drug: return new Drug(itemModel);
                case ItemEntityType.Dice: return new Dice(itemModel);
                case ItemEntityType.Watch: return new Watch(itemModel);
                case ItemEntityType.Cloth: return new Cloth(itemModel);
                case ItemEntityType.Transmitter: return new Transmitter(itemModel);
                case ItemEntityType.Cellphone: return new Cellphone(itemModel);
                case ItemEntityType.Tuning: return new Tuning(itemModel);

                default:
                    throw new NotSupportedException($"Podany typ przedmiotu {itemModel.ItemEntityType} nie jest obsługiwany.");
            }
        }
    }
}
