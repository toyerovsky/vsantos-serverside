using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Mdt;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class CriminalCasesRepository : Repository<RoleplayContext, CriminalCaseModel>, IJoinableRepository<CriminalCaseModel>
    {
        public CriminalCasesRepository(RoleplayContext context) : base(context)
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