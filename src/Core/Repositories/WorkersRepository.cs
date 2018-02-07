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
    public class WorkersRepository : IRepository<WorkerModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(WorkerModel model) => Context.Workers.Add(model);

        public bool Contains(WorkerModel model)
        {
            return Context.Workers.Any(worker => worker.Id == model.Id);
        }

        public void Update(WorkerModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var worker = Context.Workers.Find(id);
            Context.Workers.Remove(worker);
        }

        public WorkerModel Get(long id) => GetAll().Single(g => g.Id == id);

        public IEnumerable<WorkerModel> GetAll()
        {
            return Context.Workers
                .Include(worker => worker.Character)
                    .ThenInclude(character => character.AccountModel)
                .Include(worker => worker.Group)
                    .ThenInclude(group => group.BossCharacter)
                .Include(worker => worker.Group)
                    .ThenInclude(group => group.Workers)
                        .ThenInclude(group => group.Character)
                .ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}