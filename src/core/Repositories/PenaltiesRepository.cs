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
using VRP.Core.Database.Models.Account;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class PenaltiesRepository : Repository<RoleplayContext, PenaltyModel>
    {
        private readonly RoleplayContext _context;

        public PenaltiesRepository(RoleplayContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public PenaltiesRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public void Insert(PenaltyModel model)
        {
            if ((model.Account?.Id ?? 0) != 0)
                _context.Attach(model.Account);

            _context.Penaltlies.Add(model);
        }
        public override PenaltyModel Get(int id) => GetAll(penalty => penalty.Id == id).SingleOrDefault();

        public override PenaltyModel Get(Expression<Func<PenaltyModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override PenaltyModel GetNoRelated(Expression<Func<PenaltyModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public override IEnumerable<PenaltyModel> GetAll(Expression<Func<PenaltyModel, bool>> expression = null)
        {
            IQueryable<PenaltyModel> penatlies = expression != null ?
                _context.Penaltlies.Where(expression) :
                _context.Penaltlies;

            return penatlies
                .Include(penatly => penatly.Account);
        }

        public override IEnumerable<PenaltyModel> GetAllNoRelated(Expression<Func<PenaltyModel, bool>> expression = null)
        {
            IQueryable<PenaltyModel> penatlies = expression != null ?
                _context.Penaltlies.Where(expression) :
                _context.Penaltlies;

            return penatlies;
        }
    }
}