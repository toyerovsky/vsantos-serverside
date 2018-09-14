using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.CrimeBot;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class CrimeBotItemsRepository : Repository<RoleplayContext, CrimeBotItemModel>, IJoinableRepository<CrimeBotItemModel>
    {
        public CrimeBotItemsRepository(RoleplayContext context) : base(context)
        {
        }

        public override CrimeBotItemModel Get(Expression<Func<CrimeBotItemModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<CrimeBotItemModel> GetAll(Expression<Func<CrimeBotItemModel, bool>> expression = null)
        {
            IEnumerable<CrimeBotItemModel> crimeBotItems = expression != null ?
                Context.CrimeBotItems.Where(expression) :
                Context.CrimeBotItems;

            return crimeBotItems;
        }

        public CrimeBotItemModel JoinAndGet(int id) =>
            JoinAndGetAll(crimeBotItem => crimeBotItem.Id == id).SingleOrDefault();

        public async Task<CrimeBotItemModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public CrimeBotItemModel JoinAndGet(Expression<Func<CrimeBotItemModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public async Task<CrimeBotItemModel> JoinAndGetAsync(Expression<Func<CrimeBotItemModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        public IEnumerable<CrimeBotItemModel> JoinAndGetAll(Expression<Func<CrimeBotItemModel, bool>> expression)
        {
            IQueryable<CrimeBotItemModel> crimeBotItems = expression != null ?
                Context.CrimeBotItems.Where(expression) :
                Context.CrimeBotItems;

            return crimeBotItems
                .Include(crimeBotItem => crimeBotItem.ItemTemplateModel);
        }

        public async Task<IEnumerable<CrimeBotItemModel>> JoinAndGetAllAsync(Expression<Func<CrimeBotItemModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }
    }
}