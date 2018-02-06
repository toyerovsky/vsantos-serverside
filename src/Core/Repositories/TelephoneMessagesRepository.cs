using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serverside.Core.Database;
using Serverside.Core.Database.Models;
using Serverside.Core.Interfaces;

namespace Serverside.Core.Repositories
{
    public class TelephoneMessagesRepository : IRepository<TelephoneMessageModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(TelephoneMessageModel model) => Context.TelephoneMessages.Add(model);

        public bool Contains(TelephoneMessageModel model)
        {
            return Context.TelephoneMessages.Contains(model);
        }

        public void Update(TelephoneMessageModel model)
        {
            Context.Entry(model).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            var account = Context.TelephoneMessages.Find(id);
            Context.TelephoneMessages.Remove(account);
        }

        public TelephoneMessageModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<TelephoneMessageModel> GetAll()
        {
            return Context.TelephoneMessages
                .Include(message => message.Cellphone)
                    .ThenInclude(cellphone => cellphone.Creator)
                .Include(message => message.Cellphone)
                    .ThenInclude(cellphone => cellphone.Character)
                .ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}