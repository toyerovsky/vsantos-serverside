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
using VRP.Core.Database.Models;
using VRP.Core.Interfaces;

namespace VRP.Core.Repositories
{
    public class WorkersRepository : IRepository<WorkerModel>
    {
        private readonly RoleplayContext _context;

        public WorkersRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public WorkersRepository() : this(RolePlayContextFactory.NewContext())
        {
        }

        public void Insert(WorkerModel model)
        {
            if ((model.Character?.Id ?? 0) != 0)
                _context.Attach(model.Character);

            if ((model.Group?.Id ?? 0) != 0)
                _context.Attach(model.Group);

            _context.Workers.Add(model);
        }

        public bool Contains(WorkerModel model)
        {
            return _context.Workers.Any(worker => worker.Id == model.Id);
        }

        public void Update(WorkerModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            WorkerModel worker = _context.Workers.Find(id);
            _context.Workers.Remove(worker);
        }

        public WorkerModel Get(int id) => GetAll(worker => worker.Id == id).SingleOrDefault();

        public WorkerModel GetNoRelated(int id)
        {
            WorkerModel worker = _context.Workers.Find(id);
            return worker;
        }

        public WorkerModel Get(Expression<Func<WorkerModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public WorkerModel GetNoRelated(Expression<Func<WorkerModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public IEnumerable<WorkerModel> GetAll(Expression<Func<WorkerModel, bool>> expression = null)
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

        public IEnumerable<WorkerModel> GetAllNoRelated(Expression<Func<WorkerModel, bool>> expression = null)
        {
            IQueryable<WorkerModel> workers = expression != null ?
                _context.Workers.Where(expression) :
                _context.Workers;

            return workers;
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}