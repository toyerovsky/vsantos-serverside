/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */


using VRP.Core.Database.Models;
using VRP.Core.Repositories;
using VRP.Serverside.Entities.Interfaces;

namespace VRP.Serverside.Entities.Core.Item
{
    public abstract class ItemEntity : IOfferable
    {
        public int Id => DbModel.Id;
        public string Name => DbModel.Name;

        protected ItemModel DbModel { get; }
        
        public virtual string ItemInfo => $"Ten przedmiot to: {DbModel.Name} o Id: {DbModel.Id}";

        public virtual string UseInfo { get; }
    
        protected ItemEntity(ItemModel itemModel)
        {
            DbModel = itemModel;
        }

        public virtual void UseItem(AccountEntity player)
        {
            //TODO Pomysł, można tutaj dopisywać wszystkie logi używania przedmiotów
        }

        protected virtual void Save()
        {
            using (ItemsRepository repository = new ItemsRepository())
            {
                repository.Update(DbModel);
                repository.Save();
            }
        }

        protected virtual void Delete()
        {
            using (ItemsRepository repository = new ItemsRepository())
            {
                repository.Delete(DbModel.Id);
                repository.Save();
            }
        }

        public void Offer(CharacterEntity seller, CharacterEntity getter, decimal money)
        {
            throw new System.NotImplementedException();
        }
    }
}