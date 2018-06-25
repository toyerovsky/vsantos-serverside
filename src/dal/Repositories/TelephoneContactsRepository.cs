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
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Telephone;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class TelephoneContactsRepository : Repository<RoleplayContext, TelephoneContactModel>, IJoinableRepository<TelephoneContactModel>
    {
        public TelephoneContactsRepository(RoleplayContext context) : base(context)
        {
        }

        public override void Insert(TelephoneContactModel model)
        {
            if ((model.Cellphone?.Id ?? 0) != 0)
                Context.Attach(model.Cellphone);

            Context.TelephoneContacts.Add(model);
        }

        public TelephoneContactModel JoinAndGet(int id) => JoinAndGetAll(telephoneContact => telephoneContact.Id == id).SingleOrDefault();

        public TelephoneContactModel JoinAndGet(Expression<Func<TelephoneContactModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<TelephoneContactModel> JoinAndGetAll(Expression<Func<TelephoneContactModel, bool>> expression = null)
        {
            IQueryable<TelephoneContactModel> telephoneContacts = expression != null ?
                Context.TelephoneContacts.Where(expression) :
                Context.TelephoneContacts;

            return telephoneContacts
                .Include(contact => contact.Cellphone)
                .Include(contact => contact.Cellphone)
                    .ThenInclude(cellphone => cellphone.Character);
        }

        public override TelephoneContactModel Get(Func<TelephoneContactModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<TelephoneContactModel> GetAll(Func<TelephoneContactModel, bool> func = null)
        {
            IEnumerable<TelephoneContactModel> telephoneContacts = func != null ?
                Context.TelephoneContacts.Where(func) :
                Context.TelephoneContacts;

            return telephoneContacts;
        }
    }
}