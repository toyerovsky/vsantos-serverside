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
    public class CrimeBotsRepository : IRepository<CrimeBotModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(CrimeBotModel model) => Context.CrimeBots.Add(model);

        public bool Contains(CrimeBotModel model)
        {
            return Context.CrimeBots.Any(crimeBot => crimeBot.Id == model.Id);
        }

        public void Update(CrimeBotModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var crimeBot = Context.CrimeBots.Find(id);
            Context.CrimeBots.Remove(crimeBot);
        }

        public CrimeBotModel Get(long id) => GetAll().Single(c => c.Id == id);

        public CrimeBotModel Get(GroupModel model) => GetAll().Single(c => c.GroupModel.Id == model.Id);

        public IEnumerable<CrimeBotModel> GetAll()
        {
            return Context.CrimeBots.Include(cb => cb.Creator)
                .Include(crimeBot => crimeBot.GroupModel)
                    .ThenInclude(group => group.BossCharacter)
                .Include(crimeBot => crimeBot.GroupModel)
                    .ThenInclude(group => group.Workers)
                .ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}