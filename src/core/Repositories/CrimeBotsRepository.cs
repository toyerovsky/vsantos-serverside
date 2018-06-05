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
using VRP.Core.Database.Models.CrimeBot;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class CrimeBotsRepository : Repository<RoleplayContext, CrimeBotModel>
    {
        private readonly RoleplayContext _context;

        public CrimeBotsRepository(RoleplayContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public CrimeBotsRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override void Insert(CrimeBotModel model)
        {
            if ((model.GroupModel?.Id ?? 0) != 0)
                _context.Attach(model.GroupModel);

            _context.CrimeBots.Add(model);
        }

        public override CrimeBotModel Get(int id) => GetAll(crimeBot => crimeBot.Id == id).SingleOrDefault();

        public override CrimeBotModel Get(Expression<Func<CrimeBotModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override CrimeBotModel GetNoRelated(Expression<Func<CrimeBotModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public override IEnumerable<CrimeBotModel> GetAll(Expression<Func<CrimeBotModel, bool>> expression = null)
        {
            IQueryable<CrimeBotModel> crimeBots = expression != null ?
                _context.CrimeBots.Where(expression) :
                _context.CrimeBots;

            return crimeBots
                .Include(crimeBot => crimeBot.GroupModel)
                    .ThenInclude(group => group.BossCharacter)
                .Include(crimeBot => crimeBot.GroupModel)
                    .ThenInclude(group => group.Workers);
        }

        public override IEnumerable<CrimeBotModel> GetAllNoRelated(Expression<Func<CrimeBotModel, bool>> expression = null)
        {
            IQueryable<CrimeBotModel> crimeBots = expression != null ?
                _context.CrimeBots.Where(expression) :
                _context.CrimeBots;

            return crimeBots;
        }
    }
}