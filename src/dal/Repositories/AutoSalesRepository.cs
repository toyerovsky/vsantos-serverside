using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public override IEnumerable<AutoSaleModel> GetAll(Func<AutoSaleModel, bool> func = null)
        {
            IEnumerable<AutoSaleModel> autoSales = func != null ?
                Context.AutoSales.Where(func) :
                Context.AutoSales;

            return autoSales;
        }

        public AutoSaleModel JoinAndGet(int id) => JoinAndGetAll(autoSale => autoSale.Id == id).SingleOrDefault();

        public AutoSaleModel JoinAndGet(Expression<Func<AutoSaleModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<AutoSaleModel> JoinAndGetAll(Expression<Func<AutoSaleModel, bool>> expression)
        {
            IQueryable<AutoSaleModel> autoSales = expression != null ?
                Context.AutoSales.Where(expression) :
                Context.AutoSales;

            return autoSales
                .Include(autoSale => autoSale.BuildingModel)
                .Include(autoSale => autoSale.VehicleModel);
        }
    }
}