﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Core;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Repositories;
using VRP.Serverside.Interfaces;

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

        public virtual void UseItem(CharacterEntity character)
        {
            //TODO Pomysł, można tutaj dopisywać wszystkie logi używania przedmiotów
        }

        protected virtual void Save()
        {
            RoleplayContext ctx = Singletons.RoleplayContextFactory.Create();
            using (ItemsRepository repository = new ItemsRepository(ctx))
            {
                repository.Update(DbModel);
                repository.Save();
            }
        }

        protected virtual void Delete()
        {
            RoleplayContext ctx = Singletons.RoleplayContextFactory.Create();
            using (ItemsRepository repository = new ItemsRepository(ctx))
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