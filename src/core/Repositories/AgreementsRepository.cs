using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using VRP.Core.Database;
using VRP.Core.Database.Models.Account;
using VRP.Core.Database.Models.Agreement;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class AgreementsRepository : Repository<RoleplayContext, AgreementModel>
    {


        public override AgreementModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public override AgreementModel Get(Expression<Func<AgreementModel, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public override AgreementModel GetNoRelated(Expression<Func<AgreementModel, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<AgreementModel> GetAll(Expression<Func<AgreementModel, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<AgreementModel> GetAllNoRelated(Expression<Func<AgreementModel, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public AgreementsRepository(RoleplayContext context) : base(context)
        {
        }

        public AgreementsRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }
    }
}