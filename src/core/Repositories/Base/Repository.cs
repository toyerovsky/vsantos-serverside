/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.Core.Database;
using VRP.Core.Interfaces;

namespace VRP.Core.Repositories.Base
{
    public abstract class Repository<TContext, TEntity> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _context;

        protected Repository(TContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        /// <summary>
        /// Method doensn't include saving added entities
        /// </summary>
        /// <param name="model"></param>
        public virtual void Insert(TEntity model)
        {
            _context.Add(model);
        }

        public virtual async Task InsertAsync(TEntity model)
        {
            await _context.AddAsync(model);
        }

        public virtual bool Contains(TEntity model)
        {
            return _context.Find<TEntity>(model) != null;
        }

        public virtual async Task<bool> ContainsAsync(TEntity model)
        {
            return await _context.FindAsync<TEntity>(model) != null;
        }

        /// <summary>
        /// Updates all fields of model after Save() is called
        /// </summary>
        /// <param name="model"></param>
        public virtual void Update(TEntity model)
        {
            _context.Update(model);
        }

        /// <summary>
        /// Begins tracking of given entity in unmodified state. If SaveChanges is called it will update only fields which was updated between calling BeginUpdate() and Save()
        /// </summary>
        /// <param name="model"></param>
        public virtual void BeginUpdate(TEntity model)
        {
            _context.Attach(model);
        }

        public virtual void Delete(int id)
        {
            TEntity model = _context.Find<TEntity>(id);
            _context.Remove(model);
        }

        public abstract TEntity Get(int id);
        public abstract TEntity Get(Expression<Func<TEntity, bool>> expression);
        public abstract TEntity GetNoRelated(Expression<Func<TEntity, bool>> expression);
        public abstract IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression = null);
        public abstract IEnumerable<TEntity> GetAllNoRelated(Expression<Func<TEntity, bool>> expression = null);

        /// <summary>
        /// Get entity without eager loading navigation properties
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetNoRelated(int id)
        {
            TEntity model = _context.Find<TEntity>(id);
            return model;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            _context?.Dispose();
        }
    }
}