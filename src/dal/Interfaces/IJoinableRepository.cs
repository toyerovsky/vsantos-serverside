using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VRP.DAL.Interfaces
{
    public interface IJoinableRepository<T> : IRepository<T>
    {
        T JoinAndGet(int id);
        T JoinAndGet(Expression<Func<T, bool>> expression = null);
        IEnumerable<T> JoinAndGetAll(Expression<Func<T, bool>> expression = null);
    }
}