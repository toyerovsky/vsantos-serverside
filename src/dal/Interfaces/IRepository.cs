/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VRP.DAL.Interfaces
{
    public interface IRepository<T>
    {
        void Insert(T model);

        Task InsertAsync(T model);

        bool Contains(int id);

        Task<bool> ContainsAsync(int id);

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

        /// <summary>
        /// Removes an entity from the database when next Save() is called
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Get entity without eager loading navigation properties
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(int id);


        Task<T> GetAsync(object key);

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