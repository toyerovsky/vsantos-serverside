/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
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
        private readonly RoleplayContext _context;

        public TelephoneContactsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public TelephoneContactsRepository()
        {
            _context = RolePlayContextFactory.NewContext();
        }

        public void Insert(TelephoneContactModel model) => _context.TelephoneContacts.Add(model);

        public bool Contains(TelephoneContactModel model)
        {
            return _context.TelephoneContacts.Any(telephoneContact => telephoneContact.Id == model.Id);
        }

        public void Update(TelephoneContactModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            TelephoneContactModel contact = _context.TelephoneContacts.Find(id);
            _context.TelephoneContacts.Remove(contact);
        }

        public TelephoneContactModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<TelephoneContactModel> GetAll()
        {
            return _context.TelephoneContacts
                .Include(contact => contact.Cellphone)
                    .ThenInclude(cellphone => cellphone.Creator)
                .Include(contact => contact.Cellphone)
                    .ThenInclude(cellphone => cellphone.Character)
                .ToList();
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}