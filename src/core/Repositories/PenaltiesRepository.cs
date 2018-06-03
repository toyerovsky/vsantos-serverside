﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
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
using VRP.Core.Database.Models.Account;
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

        public PenaltiesRepository() : this(RoleplayContextFactory.NewContext())
        {
        }

        public void Insert(PenaltyModel model)
        {
            if ((model.Account?.Id ?? 0) != 0)
                _context.Attach(model.Account);

            _context.Penaltlies.Add(model);
        }

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

        public PenaltyModel Get(int id) => GetAll(penalty => penalty.Id == id).SingleOrDefault();

        public PenaltyModel GetNoRelated(int id)
        {
            PenaltyModel penatly = _context.Penaltlies.Find(id);
            return penatly;
        }

        public PenaltyModel Get(Expression<Func<PenaltyModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public PenaltyModel GetNoRelated(Expression<Func<PenaltyModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public IEnumerable<PenaltyModel> GetAll(Expression<Func<PenaltyModel, bool>> expression = null)
        {
            IQueryable<PenaltyModel> penatlies = expression != null ?
                _context.Penaltlies.Where(expression) :
                _context.Penaltlies;

            return penatlies
                .Include(penatly => penatly.Account);
        }

        public IEnumerable<PenaltyModel> GetAllNoRelated(Expression<Func<PenaltyModel, bool>> expression = null)
        {
            IQueryable<PenaltyModel> penatlies = expression != null ?
                _context.Penaltlies.Where(expression) :
                _context.Penaltlies;

            return penatlies;

        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}