/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
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
    public class PenaltiesRepository : IRepository<PenaltyModel>
    {
        private readonly RoleplayContext _context;

        public PenaltiesRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public PenaltiesRepository() : this(RolePlayContextFactory.NewContext())
        {

        }

        public void Insert(PenaltyModel model) => _context.Penaltlies.Add(model);

        public bool Contains(PenaltyModel model)
        {
            return _context.Penaltlies.Any(penatly => penatly.Id == model.Id);
        }

        public void Update(PenaltyModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            PenaltyModel penatly = _context.Penaltlies.Find(id);
            _context.Penaltlies.Remove(penatly);
        }

        public PenaltyModel Get(int id) => GetAll(b => b.Id == id).Single();

        public IEnumerable<PenaltyModel> GetAll(Expression<Func<PenaltyModel, bool>> predicate = null)
        {
            return _context.Penaltlies
                .Include(penatly => penatly.Account)
                .Include(penatly => penatly.Creator);
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}