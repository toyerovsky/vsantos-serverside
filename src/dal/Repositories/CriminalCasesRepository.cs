﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        public override async Task<CriminalCaseModel> GetAsync(Func<CriminalCaseModel, bool> func)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<CriminalCaseModel> GetAll(Func<CriminalCaseModel, bool> func = null)
        {
            IEnumerable<CriminalCaseModel> criminalCases = func != null ?
                Context.CriminalCases.Where(func) :
                Context.CriminalCases;

            return criminalCases;
        }

        public CriminalCaseModel JoinAndGet(int id) =>
            JoinAndGetAll(criminalCase => criminalCase.Id == id).SingleOrDefault();

        public async Task<CriminalCaseModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public CriminalCaseModel JoinAndGet(Expression<Func<CriminalCaseModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public async Task<CriminalCaseModel> JoinAndGetAsync(Expression<Func<CriminalCaseModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        public IEnumerable<CriminalCaseModel> JoinAndGetAll(Expression<Func<CriminalCaseModel, bool>> expression)
        {
            IQueryable<CriminalCaseModel> criminalCases = expression != null ?
                Context.CriminalCases.Where(expression) :
                Context.CriminalCases;

            return criminalCases
                .Include(criminalCase => criminalCase.InvolvedPeople)
                .Include(criminalCase => criminalCase.InvolvedVehicles);
        }

        public async Task<IEnumerable<CriminalCaseModel>> JoinAndGetAllAsync(Expression<Func<CriminalCaseModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }
    }
}