using System;
using System.Collections.Generic;
using System.Linq;
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

        public override ItemTemplateModel Get(Func<ItemTemplateModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<ItemTemplateModel> GetAll(Func<ItemTemplateModel, bool> func = null)
        {
            IEnumerable<ItemTemplateModel> itemTemplates = func != null ?
                Context.ItemTemplates.Where(func) :
                Context.ItemTemplates;

            return itemTemplates;
        }
    }
}