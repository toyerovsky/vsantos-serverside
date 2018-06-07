﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VRP.Core.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        /// <summary>
        /// Method doensn't include saving added entities
        /// </summary>
        /// <param name="model"></param>
        void Insert(T model);

        Task InsertAsync(T model);

        bool Contains(T model);

        Task<bool> ContainsAsync(T model);

        /// <summary>
        /// Updates all fields of model after Save() is called
        /// </summary>
        /// <param name="model"></param>
        void Update(T model);

        /// <summary>
        /// Begins tracking of given entity in unmodified state. If SaveChanges is called it will update only fields which was updated between calling BeginUpdate() and Save()
        /// </summary>
        /// <param name="model"></param>
        void BeginUpdate(T model);

        void Delete(int id);

        /// <summary>
        /// Get entity without eager loading navigation properties
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(int id);

        /// <summary>
        /// Get entity without eager loading navigation properties
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        T Get(Func<T, bool> func);

        /// <summary>
        /// Get entities without eager loading navigation properties
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        IEnumerable<T> GetAll(Func<T, bool> func = null);

        void Save();

        Task<int> SaveAsync();
    }
}