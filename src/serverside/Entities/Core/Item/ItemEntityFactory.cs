using System;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
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
