using System;
using Serverside.Core.Database.Models;
using Serverside.Entities.Interfaces;

namespace Serverside.Entities.Core.Item
{
    public class ItemEntityFactory : IEntityFactory<ItemEntity, ItemModel>
    {
        public ItemEntity Create(ItemModel itemModel)
        {
            switch (itemModel.ItemType)
            {
                case ItemType.Food: return new Food(itemModel);
                case ItemType.Weapon: return new Weapon(itemModel);
                case ItemType.WeaponClip: return new WeaponClip(itemModel);
                case ItemType.Mask: return new Mask(itemModel);
                case ItemType.Drug: return new Drug(itemModel);
                case ItemType.Dice: return new Dice(itemModel);
                case ItemType.Watch: return new Watch(itemModel);
                case ItemType.Cloth: return new Cloth(itemModel);
                case ItemType.Transmitter: return new Transmitter(itemModel);
                case ItemType.Cellphone: return new Cellphone(itemModel);
                case ItemType.Tuning: return new Tuning(itemModel);

                default:
                    throw new NotSupportedException($"Podany typ przedmiotu {itemModel.ItemType} nie jest obsługiwany.");
            }
        }
    }
}
