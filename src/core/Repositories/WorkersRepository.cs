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
using VRP.Core.Database.Models.Group;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class WorkersRepository : Repository<RoleplayContext, WorkerModel>
    {
        private readonly RoleplayContext _context;

        public WorkersRepository(RoleplayContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public WorkersRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override void Insert(WorkerModel model)
        {
            if ((model.Character?.Id ?? 0) != 0)
                _context.Attach(model.Character);

            if ((model.Group?.Id ?? 0) != 0)
                _context.Attach(model.Group);

            _context.Workers.Add(model);
        }

        public override WorkerModel Get(int id) => GetAll(worker => worker.Id == id).SingleOrDefault();

        public override WorkerModel Get(Expression<Func<WorkerModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override WorkerModel GetNoRelated(Expression<Func<WorkerModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public override IEnumerable<WorkerModel> GetAll(Expression<Func<WorkerModel, bool>> expression = null)
        {
            IQueryable<WorkerModel> workers = expression != null ?
                _context.Workers.Where(expression) :
                _context.Workers;

            return workers
                .Include(worker => worker.Character)
                    .ThenInclude(character => character.Account)
                .Include(worker => worker.Group)
                    .ThenInclude(group => group.BossCharacter)
                .Include(worker => worker.Group)
                    .ThenInclude(group => group.Workers)
                        .ThenInclude(group => group.Character);
        }

        public override IEnumerable<WorkerModel> GetAllNoRelated(Expression<Func<WorkerModel, bool>> expression = null)
        {
            IQueryable<WorkerModel> workers = expression != null ?
                _context.Workers.Where(expression) :
                _context.Workers;

            return workers;
        }
    }
}