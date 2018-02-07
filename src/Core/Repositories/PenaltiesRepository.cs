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
    public class PenaltiesRepository : IRepository<PenaltyModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(PenaltyModel model) => Context.Penaltlies.Add(model);

        public bool Contains(PenaltyModel model)
        {
            return Context.Penaltlies.Any(penatly => penatly.Id == model.Id);
        }

        public void Update(PenaltyModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var penatly = Context.Penaltlies.Find(id);
            Context.Penaltlies.Remove(penatly);
        }

        public PenaltyModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<PenaltyModel> GetAll()
        {
            return Context.Penaltlies
                .Include(penatly => penatly.Account)
                .Include(penatly => penatly.Creator).ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}