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
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class WorkersRepository : Repository<RoleplayContext, WorkerModel>, IJoinableRepository<WorkerModel>
    {
        public WorkersRepository(RoleplayContext context) : base(context)
        {
        }

        public override void Insert(WorkerModel model)
        {
            if ((model.Character?.Id ?? 0) != 0)
                Context.Attach(model.Character);

            if ((model.Group?.Id ?? 0) != 0)
                Context.Attach(model.Group);

            Context.Workers.Add(model);
        }

        public WorkerModel JoinAndGet(int id) => JoinAndGetAll(worker => worker.Id == id).SingleOrDefault();

        public WorkerModel JoinAndGet(Expression<Func<WorkerModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<WorkerModel> JoinAndGetAll(Expression<Func<WorkerModel, bool>> expression = null)
        {
            IQueryable<WorkerModel> workers = expression != null ?
                Context.Workers.Where(expression) :
                Context.Workers;

            return workers
                .Include(worker => worker.Character)
                    .ThenInclude(character => character.Account)
                .Include(worker => worker.Group)
                    .ThenInclude(group => group.BossCharacter)
                .Include(worker => worker.Group)
                    .ThenInclude(group => group.Workers)
                        .ThenInclude(group => group.Character);
        }

        public override WorkerModel Get(Func<WorkerModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<WorkerModel> GetAll(Func<WorkerModel, bool> func = null)
        {
            IEnumerable<WorkerModel> workers = func != null ?
                Context.Workers.Where(func) :
                Context.Workers;

            return workers;
        }
    }
}