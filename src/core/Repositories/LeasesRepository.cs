using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VRP.Core.Database;
using VRP.Core.Database.Models.Agreement;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class LeasesRepository : Repository<RoleplayContext, LeaseModel>
    {
        public LeasesRepository(RoleplayContext context) : base(context)
        {
        }

        public override LeaseModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public override LeaseModel Get(Expression<Func<LeaseModel, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public override LeaseModel GetNoRelated(Expression<Func<LeaseModel, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<LeaseModel> GetAll(Expression<Func<LeaseModel, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<LeaseModel> GetAllNoRelated(Expression<Func<LeaseModel, bool>> expression = null)
        {
            throw new NotImplementedException();
        }
    }
}