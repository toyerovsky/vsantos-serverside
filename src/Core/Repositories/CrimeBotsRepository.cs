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
            return Context.CrimeBots.Contains(model);
        }

        public void Update(CrimeBotModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var account = Context.Accounts.Find(id);
            Context.Accounts.Remove(account);
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