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
    public class TelephoneMessagesRepository : Repository<RoleplayContext, TelephoneMessageModel>
    {
        private readonly RoleplayContext _context;

        public TelephoneMessagesRepository(RoleplayContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public TelephoneMessagesRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public void Insert(TelephoneMessageModel model)
        {
            if ((model.Cellphone?.Id ?? 0) != 0)
                _context.Attach(model.Cellphone);

            _context.TelephoneMessages.Add(model);
        }

        public override TelephoneMessageModel Get(int id) => GetAll(telephoneMessage => telephoneMessage.Id == id).SingleOrDefault();

        public override TelephoneMessageModel GetNoRelated(int id)
        {
            TelephoneMessageModel message = _context.TelephoneMessages.Find(id);
            return message;
        }

        public override TelephoneMessageModel Get(Expression<Func<TelephoneMessageModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override TelephoneMessageModel GetNoRelated(Expression<Func<TelephoneMessageModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public override IEnumerable<TelephoneMessageModel> GetAll(Expression<Func<TelephoneMessageModel, bool>> expression = null)
        {
            IQueryable<TelephoneMessageModel> telephoneMessages = expression != null ?
                _context.TelephoneMessages.Where(expression) :
                _context.TelephoneMessages;

            return telephoneMessages
                .Include(message => message.Cellphone)
                .Include(message => message.Cellphone)
                    .ThenInclude(cellphone => cellphone.Character);
        }

        public override IEnumerable<TelephoneMessageModel> GetAllNoRelated(Expression<Func<TelephoneMessageModel, bool>> expression = null)
        {
            IQueryable<TelephoneMessageModel> telephoneMessages = expression != null ?
                _context.TelephoneMessages.Where(expression) :
                _context.TelephoneMessages;

            return telephoneMessages;
        }
    }
}