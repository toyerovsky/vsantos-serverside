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
using VRP.DAL.Interfaces;

namespace VRP.DAL.Repositories.Base
{
    public abstract class Repository<TContext, TEntity> : IRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        protected readonly TContext Context;

        protected Repository(TContext context)
        {
            Context = context ?? throw new ArgumentException(nameof(Context));
        }

        /// <summary>
        /// Method doesn't include saving added entities
        /// </summary>
        /// <param name="model"></param>
        public virtual void Insert(TEntity model)
        {
            Context.Set<TEntity>().Add(model);
        }

        public virtual async Task InsertAsync(TEntity model)
        {
            await Context.Set<TEntity>().AddAsync(model);
        }

        public virtual bool Contains(int id)
        {
            return Context.Find<TEntity>(id) != null;
        }

        public virtual async Task<bool> ContainsAsync(int id)
        {
            return await Context.FindAsync<TEntity>(id) != null;
        }

        /// <summary>
        /// Updates all fields of model after Save() is called
        /// </summary>
        /// <param name="model"></param>
        public virtual void Update(TEntity model)
        {
            Context.Update(model);
        }

        /// <summary>
        /// Begins tracking of given entity in unmodified state. If SaveChanges is called it will update only fields which was updated between calling BeginUpdate() and Save()
        /// </summary>
        /// <param name="model"></param>
        public virtual void BeginUpdate(TEntity model)
        {
            Context.Attach(model);
        }

        public virtual void Delete(int id)
        {
            TEntity model = Context.Find<TEntity>(id);
            Context.Remove(model);
        }

        public async Task DeleteAsync(int id)
        {
            Context.Remove(await Context.FindAsync<TEntity>(id));
        }

        /// <summary>
        /// Get entity without eager loading navigation properties
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity Get(int id)
        {
            TEntity model = Context.Find<TEntity>(id);
            return model;
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await Context.FindAsync<TEntity>(id);
        }

        /// <summary>
        /// Get entity without eager loading navigation properties
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public abstract TEntity Get(Expression<Func<TEntity, bool>> expression);

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await GetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get entities without eager loading navigation properties
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public abstract IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> expression = null);

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression = null)
        {
            return await GetAll(expression).AsQueryable().ToArrayAsync();
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}