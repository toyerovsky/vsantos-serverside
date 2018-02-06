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
    public class BuildingsRepository : IRepository<BuildingModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(BuildingModel model) => Context.Buildings.Add(model);

        public bool Contains(BuildingModel model)
        {
            return Context.Buildings.Contains(model);
        }

        public void Update(BuildingModel model)
        {
            Context.Entry(model).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            var account = Context.Buildings.Find(id);
            Context.Buildings.Remove(account);
        }

        public BuildingModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<BuildingModel> GetAll()
        {
            return Context.Buildings
                .Include(building => building.Creator)
                .Include(building => building.Character)
                .Include(building => building.Group)
                .Include(building => building.Items)
                    .ThenInclude(item => item.Creator)
                .ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}