﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Misc;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class AutoSalesRepository : Repository<RoleplayContext, AutoSaleModel>, IJoinableRepository<AutoSaleModel>
    {
        public AutoSalesRepository(RoleplayContext context) : base(context)
        {
        }

        public override AutoSaleModel Get(Func<AutoSaleModel, bool> func) => GetAll(func).FirstOrDefault();

        public override async Task<AutoSaleModel> GetAsync(Func<AutoSaleModel, bool> func)
        {
            return await GetAll(func).AsQueryable().FirstOrDefaultAsync();
        }

        public override IEnumerable<AutoSaleModel> GetAll(Func<AutoSaleModel, bool> func = null)
        {
            IEnumerable<AutoSaleModel> autoSales = func != null ?
                Context.AutoSales.Where(func) :
                Context.AutoSales;

            return autoSales;
        }

        public AutoSaleModel JoinAndGet(int id) => JoinAndGetAll(autoSale => autoSale.Id == id).SingleOrDefault();

        public async Task<AutoSaleModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(autoSale => autoSale.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public AutoSaleModel JoinAndGet(Expression<Func<AutoSaleModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public async Task<AutoSaleModel> JoinAndGetAsync(Expression<Func<AutoSaleModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        public IEnumerable<AutoSaleModel> JoinAndGetAll(Expression<Func<AutoSaleModel, bool>> expression)
        {
            IQueryable<AutoSaleModel> autoSales = expression != null ?
                Context.AutoSales.Where(expression) :
                Context.AutoSales;

            return autoSales
                .Include(autoSale => autoSale.BuildingModel)
                .Include(autoSale => autoSale.VehicleModel);
        }

        public async Task<IEnumerable<AutoSaleModel>> JoinAndGetAllAsync(Expression<Func<AutoSaleModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }
    }
}