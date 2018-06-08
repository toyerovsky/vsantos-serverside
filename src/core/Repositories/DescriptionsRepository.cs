using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models.Misc;
using VRP.Core.Interfaces;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class DescriptionsRepository : Repository<RoleplayContext, DescriptionModel>, IJoinableRepository<DescriptionModel>
    {
        public DescriptionsRepository(RoleplayContext context) : base(context)
        {
        }

        public override DescriptionModel Get(Func<DescriptionModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<DescriptionModel> GetAll(Func<DescriptionModel, bool> func = null)
        {
            IEnumerable<DescriptionModel> descriptions = func != null ?
                Context.Descriptions.Where(func) :
                Context.Descriptions;

            return descriptions;
        }

        public DescriptionModel JoinAndGet(int id) =>
            JoinAndGetAll(descriptionModel => descriptionModel.Id == id).SingleOrDefault();

        public DescriptionModel JoinAndGet(Expression<Func<DescriptionModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<DescriptionModel> JoinAndGetAll(Expression<Func<DescriptionModel, bool>> expression)
        {
            IQueryable<DescriptionModel> descriptions = expression != null ?
                Context.Descriptions.Where(expression) :
                Context.Descriptions;

            return descriptions
                .Include(description => description.Character)
                .Include(description => description.Vehicle);
        }
    }
}