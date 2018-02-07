﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
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
    public class TelephoneContactsRepository : IRepository<TelephoneContactModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(TelephoneContactModel model) => Context.TelephoneContacts.Add(model);

        public bool Contains(TelephoneContactModel model)
        {
            return Context.TelephoneContacts.Any(telephoneContact => telephoneContact.Id == model.Id);
        }

        public void Update(TelephoneContactModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var contact = Context.TelephoneContacts.Find(id);
            Context.TelephoneContacts.Remove(contact);
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