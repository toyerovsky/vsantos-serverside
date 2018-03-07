/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;

namespace Serverside.Core.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        void Insert(T model);
        bool Contains(T model);
        void Update(T model);
        void Delete(long model);
        T Get(long id);
        IEnumerable<T> GetAll();
        void Save();
    }
}