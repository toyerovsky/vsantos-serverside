/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models;
using VRP.Core.Interfaces;

namespace VRP.Core.Repositories
{
    public class TelephoneContactsRepository : IRepository<TelephoneContactModel>
    {
        private readonly RoleplayContext _context;

        public TelephoneContactsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public TelephoneContactsRepository() : this(RolePlayContextFactory.NewContext())
        {
        }

        public void Insert(TelephoneContactModel model)
        {
            if ((model.Cellphone?.Id ?? 0) != 0)
                _context.Attach(model.Cellphone);

            _context.TelephoneContacts.Add(model);
        }

        public bool Contains(TelephoneContactModel model)
        {
            return _context.TelephoneContacts.Any(telephoneContact => telephoneContact.Id == model.Id);
        }

        public void Update(TelephoneContactModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            TelephoneContactModel contact = _context.TelephoneContacts.Find(id);
            _context.TelephoneContacts.Remove(contact);
        }

        public TelephoneContactModel Get(int id) => GetAll(b => b.Id == id).SingleOrDefault();

        public TelephoneContactModel GetNoRelated(int id) => GetNoRelated(b => b.Id == id).SingleOrDefault();

        public IEnumerable<TelephoneContactModel> GetAll(Expression<Func<TelephoneContactModel, bool>> expression = null)
        {
            IQueryable<TelephoneContactModel> telephoneContacts = expression != null ?
                _context.TelephoneContacts.Where(expression) :
                _context.TelephoneContacts;

            return telephoneContacts
                .Include(contact => contact.Cellphone)
                .Include(contact => contact.Cellphone)
                    .ThenInclude(cellphone => cellphone.Character);
        }

        public IEnumerable<TelephoneContactModel> GetNoRelated(Expression<Func<TelephoneContactModel, bool>> expression = null)
        {
            IQueryable<TelephoneContactModel> telephoneContacts = expression != null ?
                _context.TelephoneContacts.Where(expression) :
                _context.TelephoneContacts;

            return telephoneContacts;
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}