/* Copyright (C) Przemysław Postrach - All Rights Reserved
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
    public class TelephoneMessagesRepository : IRepository<TelephoneMessageModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(TelephoneMessageModel model) => Context.TelephoneMessages.Add(model);

        public bool Contains(TelephoneMessageModel model)
        {
            return Context.TelephoneMessages.Any(telephoneContact => telephoneContact.Id == model.Id);
        }

        public void Update(TelephoneMessageModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var message = Context.TelephoneMessages.Find(id);
            Context.TelephoneMessages.Remove(message);
        }

        public TelephoneMessageModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<TelephoneMessageModel> GetAll()
        {
            return Context.TelephoneMessages
                .Include(message => message.Cellphone)
                    .ThenInclude(cellphone => cellphone.Creator)
                .Include(message => message.Cellphone)
                    .ThenInclude(cellphone => cellphone.Character)
                .ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}