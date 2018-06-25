using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public LeaseModel JoinAndGet(Expression<Func<LeaseModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<LeaseModel> JoinAndGetAll(Expression<Func<LeaseModel, bool>> expression)
        {
            IQueryable<LeaseModel> leases = expression != null ?
                Context.Leases.Where(expression) :
                Context.Leases;

            return leases
                .Include(lease => lease.AgreementModel)
                .Include(lease => lease.Building)
                .Include(lease => lease.Vehicle);
        }

        public override LeaseModel Get(Func<LeaseModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<LeaseModel> GetAll(Func<LeaseModel, bool> func = null)
        {
            IEnumerable<LeaseModel> leases = func != null ?
                Context.Leases.Where(func) :
                Context.Leases;

            return leases;
        }
    }
}