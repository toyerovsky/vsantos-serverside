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
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class TelephoneMessagesRepository : Repository<RoleplayContext, TelephoneMessageModel>
    {
        public TelephoneMessagesRepository(RoleplayContext context) : base(context)
        {
        }

        public TelephoneMessageModel JoinAndGet(int id) => JoinAndGetAll(telephoneMessage => telephoneMessage.Id == id).SingleOrDefault();

        public TelephoneMessageModel JoinAndGet(Expression<Func<TelephoneMessageModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<TelephoneMessageModel> JoinAndGetAll(Expression<Func<TelephoneMessageModel, bool>> expression = null)
        {
            IQueryable<TelephoneMessageModel> telephoneMessages = expression != null ?
                Context.TelephoneMessages.Where(expression) :
                Context.TelephoneMessages;

            return telephoneMessages
                .Include(message => message.Cellphone)
                .Include(message => message.Cellphone)
                    .ThenInclude(cellphone => cellphone.Character);
        }

        public override TelephoneMessageModel Get(Func<TelephoneMessageModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<TelephoneMessageModel> GetAll(Func<TelephoneMessageModel, bool> func = null)
        {
            IEnumerable<TelephoneMessageModel> telephoneMessages = func != null ?
                Context.TelephoneMessages.Where(func) :
                Context.TelephoneMessages;

            return telephoneMessages;
        }
    }
}