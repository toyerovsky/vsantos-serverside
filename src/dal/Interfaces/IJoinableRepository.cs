using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VRP.DAL.Interfaces
{
    public interface IJoinableRepository<T> : IRepository<T>
    {
        T JoinAndGet(int id);
        Task<T> JoinAndGetAsync(int id);
        T JoinAndGet(Expression<Func<T, bool>> expression = null);
        Task<T> JoinAndGetAsync(Expression<Func<T, bool>> expression = null);
        IEnumerable<T> JoinAndGetAll(Expression<Func<T, bool>> expression = null);
        Task<IEnumerable<T>> JoinAndGetAllAsync(Expression<Func<T, bool>> expression = null);
    }
}