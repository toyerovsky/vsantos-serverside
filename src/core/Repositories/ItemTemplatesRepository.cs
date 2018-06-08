using System;
using System.Collections.Generic;
using System.Linq;
using VRP.Core.Database;
using VRP.Core.Database.Models.Item;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class ItemTemplatesRepository : Repository<RoleplayContext, ItemTemplateModel>
    {
        public ItemTemplatesRepository(RoleplayContext context) : base(context)
        {
        }

        public ItemTemplatesRepository() : this(Singletons.RoleplayContextFactory.Create())
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