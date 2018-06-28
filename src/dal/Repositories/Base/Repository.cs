﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
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
        /// Method doensn't include saving added entities
        /// </summary>
        /// <param name="model"></param>
        public virtual void Insert(TEntity model)
        {
            Context.Add(model);
        }

        public virtual async Task InsertAsync(TEntity model)
        {
            await Context.AddAsync(model);
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

        /// <summary>
        /// Get entity without eager loading navigation properties
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual TEntity Get(object key)
        {
            TEntity model = Context.Find<TEntity>(key);
            return model;
        }

        public virtual async Task<TEntity> GetAsync(object key)
        {
            Task<TEntity> task = Context.FindAsync<TEntity>(key);
            return await task;
        }

        /// <summary>
        /// Get entity without eager loading navigation properties
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public abstract TEntity Get(Func<TEntity, bool> func);

        /// <summary>
        /// Get entities without eager loading navigation properties
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public abstract IEnumerable<TEntity> GetAll(Func<TEntity, bool> func = null);

        public void Save()
        {
            Context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await Context.SaveChangesAsync();
        }

        public virtual void Dispose()
        {
            Context?.Dispose();
        }
    }
}