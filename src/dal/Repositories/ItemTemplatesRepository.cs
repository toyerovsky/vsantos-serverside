using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class ItemTemplatesRepository : Repository<RoleplayContext, ItemTemplateModel>
    {
        public ItemTemplatesRepository(RoleplayContext context) : base(context)
        {
        }

        public override ItemTemplateModel Get(Expression<Func<ItemTemplateModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<ItemTemplateModel> GetAll(Expression<Func<ItemTemplateModel, bool>> expression = null)
        {
            IQueryable<ItemTemplateModel> itemTemplates = expression != null ?
                Context.ItemTemplates.Where(expression) :
                Context.ItemTemplates;

            return itemTemplates;
        }
    }
}