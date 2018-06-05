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
using VRP.Core.Database.Models.Telephone;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class TelephoneContactsRepository : Repository<RoleplayContext, TelephoneContactModel>
    {
        private readonly RoleplayContext _context;

        public TelephoneContactsRepository(RoleplayContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public TelephoneContactsRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public void Insert(TelephoneContactModel model)
        {
            if ((model.Cellphone?.Id ?? 0) != 0)
                _context.Attach(model.Cellphone);

            _context.TelephoneContacts.Add(model);
        }

        public override TelephoneContactModel Get(int id) => GetAll(telephoneContact => telephoneContact.Id == id).SingleOrDefault();

        public override TelephoneContactModel GetNoRelated(int id)
        {
            TelephoneContactModel contact = _context.TelephoneContacts.Find(id);
            return contact;
        }

        public override TelephoneContactModel Get(Expression<Func<TelephoneContactModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override TelephoneContactModel GetNoRelated(Expression<Func<TelephoneContactModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public override IEnumerable<TelephoneContactModel> GetAll(Expression<Func<TelephoneContactModel, bool>> expression = null)
        {
            IQueryable<TelephoneContactModel> telephoneContacts = expression != null ?
                _context.TelephoneContacts.Where(expression) :
                _context.TelephoneContacts;

            return telephoneContacts
                .Include(contact => contact.Cellphone)
                .Include(contact => contact.Cellphone)
                    .ThenInclude(cellphone => cellphone.Character);
        }

        public override IEnumerable<TelephoneContactModel> GetAllNoRelated(Expression<Func<TelephoneContactModel, bool>> expression = null)
        {
            IQueryable<TelephoneContactModel> telephoneContacts = expression != null ?
                _context.TelephoneContacts.Where(expression) :
                _context.TelephoneContacts;

            return telephoneContacts;
        }
    }
}