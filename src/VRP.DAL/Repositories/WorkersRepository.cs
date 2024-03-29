﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public WorkerModel JoinAndGet(int id) => JoinAndGetAll(worker => worker.Id == id).SingleOrDefault();

        public async Task<WorkerModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public WorkerModel JoinAndGet(Expression<Func<WorkerModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public async Task<WorkerModel> JoinAndGetAsync(Expression<Func<WorkerModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

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

        public async Task<IEnumerable<WorkerModel>> JoinAndGetAllAsync(Expression<Func<WorkerModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }

        public override WorkerModel Get(Expression<Func<WorkerModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<WorkerModel> GetAll(Expression<Func<WorkerModel, bool>> expression = null)
        {
            IEnumerable<WorkerModel> workers = expression != null ?
                Context.Workers.Where(expression) :
                Context.Workers;

            return workers;
        }
    }
}