/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;

namespace VRP.Core.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        void Insert(T model);
        bool Contains(T model);
        void Update(T model);
        void Delete(int id);
        T Get(int id);
        IEnumerable<T> GetAll();
        void Save();
    }
}