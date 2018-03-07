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
    public class PenaltiesRepository : IRepository<PenaltyModel>
    {
        private readonly RoleplayContext _context;

        public PenaltiesRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public PenaltiesRepository()
        {
            _context = RolePlayContextFactory.NewContext();
        }

        public void Insert(PenaltyModel model) => _context.Penaltlies.Add(model);

        public bool Contains(PenaltyModel model)
        {
            return _context.Penaltlies.Any(penatly => penatly.Id == model.Id);
        }

        public void Update(PenaltyModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            PenaltyModel penatly = _context.Penaltlies.Find(id);
            _context.Penaltlies.Remove(penatly);
        }

        public PenaltyModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<PenaltyModel> GetAll()
        {
            return _context.Penaltlies
                .Include(penatly => penatly.Account)
                .Include(penatly => penatly.Creator).ToList();
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}