using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serverside.Core.Database;
using Serverside.Core.Database.Models;
using Serverside.Core.Interfaces;

namespace Serverside.Core.Repositories
{
    public class TelephoneContactsRepository : IRepository<TelephoneContactModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(TelephoneContactModel model) => Context.TelephoneContacts.Add(model);

        public bool Contains(TelephoneContactModel model)
        {
            return Context.TelephoneContacts.Contains(model);
        }

        public void Update(TelephoneContactModel model)
        {
            Context.Entry(model).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            var account = Context.TelephoneContacts.Find(id);
            Context.TelephoneContacts.Remove(account);
        }

        public TelephoneContactModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<TelephoneContactModel> GetAll()
        {
            return Context.TelephoneContacts
                .Include(contact => contact.Cellphone)
                    .ThenInclude(cellphone => cellphone.Creator)
                .Include(contact => contact.Cellphone)
                    .ThenInclude(cellphone => cellphone.Character)
                .ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}