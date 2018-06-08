using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models.CrimeBot;
using VRP.Core.Interfaces;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class CrimeBotItemsRepository : Repository<RoleplayContext, CrimeBotItemModel>, IJoinableRepository<CrimeBotItemModel>
    {
        public CrimeBotItemsRepository(RoleplayContext context) : base(context)
        {
        }

        public CrimeBotItemsRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override CrimeBotItemModel Get(Func<CrimeBotItemModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<CrimeBotItemModel> GetAll(Func<CrimeBotItemModel, bool> func = null)
        {
            IEnumerable<CrimeBotItemModel> crimeBotItems = func != null ?
                Context.CrimeBotItems.Where(func) :
                Context.CrimeBotItems;

            return crimeBotItems;
        }

        public CrimeBotItemModel JoinAndGet(int id) =>
            JoinAndGetAll(crimeBotItem => crimeBotItem.Id == id).SingleOrDefault();

        public CrimeBotItemModel JoinAndGet(Expression<Func<CrimeBotItemModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<CrimeBotItemModel> JoinAndGetAll(Expression<Func<CrimeBotItemModel, bool>> expression)
        {
            IQueryable<CrimeBotItemModel> crimeBotItems = expression != null ?
                Context.CrimeBotItems.Where(expression) :
                Context.CrimeBotItems;

            return crimeBotItems
                .Include(crimeBotItem => crimeBotItem.ItemTemplateModel);
        }
    }
}