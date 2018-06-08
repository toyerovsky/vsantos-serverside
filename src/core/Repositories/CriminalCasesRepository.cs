using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models.Mdt;
using VRP.Core.Interfaces;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class CriminalCasesRepository : Repository<RoleplayContext, CriminalCaseModel>, IJoinableRepository<CriminalCaseModel>
    {
        public CriminalCasesRepository(RoleplayContext context) : base(context)
        {
        }

        public CriminalCasesRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override CriminalCaseModel Get(Func<CriminalCaseModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<CriminalCaseModel> GetAll(Func<CriminalCaseModel, bool> func = null)
        {
            IEnumerable<CriminalCaseModel> criminalCases = func != null ?
                Context.CriminalCases.Where(func) :
                Context.CriminalCases;

            return criminalCases;
        }

        public CriminalCaseModel JoinAndGet(int id) =>
            JoinAndGetAll(criminalCase => criminalCase.Id == id).SingleOrDefault();

        public CriminalCaseModel JoinAndGet(Expression<Func<CriminalCaseModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<CriminalCaseModel> JoinAndGetAll(Expression<Func<CriminalCaseModel, bool>> expression)
        {
            IQueryable<CriminalCaseModel> criminalCases = expression != null ?
                Context.CriminalCases.Where(expression) :
                Context.CriminalCases;

            return criminalCases
                .Include(criminalCase => criminalCase.InvolvedPeople)
                .Include(criminalCase => criminalCase.InvolvedVehicles);
        }
    }
}