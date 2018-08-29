using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Warehouse;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class GroupWarehousesRepository : Repository<RoleplayContext, GroupWarehouseModel>, IJoinableRepository<GroupWarehouseModel>
    {
        public GroupWarehousesRepository(RoleplayContext context) : base(context)
        {
        }

        public override GroupWarehouseModel Get(Func<GroupWarehouseModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<GroupWarehouseModel> GetAll(Func<GroupWarehouseModel, bool> func = null)
        {
            IEnumerable<GroupWarehouseModel> groupWarehouses = func != null ?
                Context.GroupWarehouses.Where(func) :
                Context.GroupWarehouses;

            return groupWarehouses;
        }

        public GroupWarehouseModel JoinAndGet(int id) =>
            JoinAndGetAll(groupWarehouse => groupWarehouse.Id == id).SingleOrDefault();

        public async Task<GroupWarehouseModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public GroupWarehouseModel JoinAndGet(Expression<Func<GroupWarehouseModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public async Task<GroupWarehouseModel> JoinAndGetAsync(Expression<Func<GroupWarehouseModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        public IEnumerable<GroupWarehouseModel> JoinAndGetAll(Expression<Func<GroupWarehouseModel, bool>> expression)
        {
            IQueryable<GroupWarehouseModel> groupWarehouses = expression != null ?
                Context.GroupWarehouses.Where(expression) :
                Context.GroupWarehouses;

            return groupWarehouses
                .Include(groupWarehouse => groupWarehouse.Group)
                .Include(groupWarehouse => groupWarehouse.ItemsInWarehouse);
        }

        public async Task<IEnumerable<GroupWarehouseModel>> JoinAndGetAllAsync(Expression<Func<GroupWarehouseModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }
    }
}