﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Core.Repositories;
using Serverside.Entities.Core;

namespace Serverside.Items
{
    internal abstract class Item
    {
        public long Id { get; }
        public string Name { get; }

        protected ItemModel DbModel { get; }
        protected EventClass Events { get; }

        public virtual string ItemInfo => $"Ten przedmiot to: {DbModel.Name} o Id: {DbModel.Id}";

        public virtual string UseInfo { get; }
    
        protected Item(EventClass events, ItemModel itemModel)
        {
            Events = events;
            DbModel = itemModel;

            Id = DbModel.Id;
            Name = DbModel.Name;
        }

        public virtual void UseItem(AccountEntity player)
        {
            //TODO Pomysł, można tutaj dopisywać wszystkie logi używania przedmiotów
        }

        protected void Save()
        {
            using (ItemsRepository repository = new ItemsRepository())
            {
                repository.Update(DbModel);
                repository.Save();
            }
        }

        protected void Delete()
        {
            using (ItemsRepository repository = new ItemsRepository())
            {
                repository.Delete(DbModel.Id);
                repository.Save();
            }
        }
    }
}