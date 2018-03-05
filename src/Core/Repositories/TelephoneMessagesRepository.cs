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
    public class TelephoneMessagesRepository : IRepository<TelephoneMessageModel>
    {
        private readonly RoleplayContext _context;

        public TelephoneMessagesRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public TelephoneMessagesRepository()
        {
            _context = RolePlayContextFactory.NewContext();
        }

        public void Insert(TelephoneMessageModel model) => _context.TelephoneMessages.Add(model);

        public bool Contains(TelephoneMessageModel model)
        {
            return _context.TelephoneMessages.Any(telephoneContact => telephoneContact.Id == model.Id);
        }

        public void Update(TelephoneMessageModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            TelephoneMessageModel message = _context.TelephoneMessages.Find(id);
            _context.TelephoneMessages.Remove(message);
        }

        public TelephoneMessageModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<TelephoneMessageModel> GetAll()
        {
            return _context.TelephoneMessages
                .Include(message => message.Cellphone)
                    .ThenInclude(cellphone => cellphone.Creator)
                .Include(message => message.Cellphone)
                    .ThenInclude(cellphone => cellphone.Character)
                .ToList();
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}