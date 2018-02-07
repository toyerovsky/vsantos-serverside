/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serverside.Core.Database;
using Serverside.Core.Database.Models;
using Serverside.Core.Interfaces;

namespace Serverside.Core.Repositories
{
    public class ItemsRepository : IRepository<ItemModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(ItemModel model) => Context.Items.Add(model);

        public bool Contains(ItemModel model)
        {
            return Context.Items.Any(item => item.Id == model.Id);
        }

        public void Update(ItemModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var item = Context.Items.Find(id);
            Context.Items.Remove(item);
        }

        public ItemModel Get(long id) => GetAll().Single(i => i.Id == id);

        public IEnumerable<ItemModel> GetAll()
        {
            return Context.Items
                .Include(item => item.Creator)
                .Include(item => item.Building)
                    .ThenInclude(building => building.Creator)
                .Include(item => item.Character)
                    .ThenInclude(character => character.AccountModel)
                .Include(item => item.Group)
                    .ThenInclude(group => group.BossCharacter)
                        .ThenInclude(bossCharacter => bossCharacter.AccountModel)
                .ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}