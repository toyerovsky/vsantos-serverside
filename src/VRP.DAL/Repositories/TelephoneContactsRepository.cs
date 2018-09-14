/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public TelephoneContactModel JoinAndGet(int id) => JoinAndGetAll(telephoneContact => telephoneContact.Id == id).SingleOrDefault();
        public async Task<TelephoneContactModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public TelephoneContactModel JoinAndGet(Expression<Func<TelephoneContactModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();
        public async Task<TelephoneContactModel> JoinAndGetAsync(Expression<Func<TelephoneContactModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

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

        public async Task<IEnumerable<TelephoneContactModel>> JoinAndGetAllAsync(Expression<Func<TelephoneContactModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }

        public override TelephoneContactModel Get(Expression<Func<TelephoneContactModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<TelephoneContactModel> GetAll(Expression<Func<TelephoneContactModel, bool>> expression = null)
        {
            IQueryable<TelephoneContactModel> telephoneContacts = expression != null ?
                Context.TelephoneContacts.Where(expression) :
                Context.TelephoneContacts;

            return telephoneContacts;
        }
    }
}