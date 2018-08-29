using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Agreement;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class LeasesRepository : Repository<RoleplayContext, LeaseModel>, IJoinableRepository<LeaseModel>
    {
        public LeasesRepository(RoleplayContext context) : base(context)
        {
        }

        public LeaseModel JoinAndGet(int id) => JoinAndGetAll(lease => lease.Id == id).SingleOrDefault();
        public async Task<LeaseModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public LeaseModel JoinAndGet(Expression<Func<LeaseModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();
        public async Task<LeaseModel> JoinAndGetAsync(Expression<Func<LeaseModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        public IEnumerable<LeaseModel> JoinAndGetAll(Expression<Func<LeaseModel, bool>> expression)
        {
            IQueryable<LeaseModel> leases = expression != null ?
                Context.Leases.Where(expression) :
                Context.Leases;

            return leases
                .Include(lease => lease.Agreement)
                .Include(lease => lease.Building)
                .Include(lease => lease.Vehicle);
        }

        public async Task<IEnumerable<LeaseModel>> JoinAndGetAllAsync(Expression<Func<LeaseModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }

        public override LeaseModel Get(Func<LeaseModel, bool> func) => GetAll(func).FirstOrDefault();
        public override async Task<LeaseModel> GetAsync(Func<LeaseModel, bool> func)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<LeaseModel> GetAll(Func<LeaseModel, bool> func = null)
        {
            IEnumerable<LeaseModel> leases = func != null ?
                Context.Leases.Where(func) :
                Context.Leases;

            return leases;
        }
    }
}