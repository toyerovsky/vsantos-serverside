using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models.Account;
using VRP.Core.Database.Models.Agreement;
using VRP.Core.Interfaces;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class AgreementsRepository : Repository<RoleplayContext, AgreementModel>, IJoinableRepository<AgreementModel>
    {
        public AgreementsRepository(RoleplayContext context) : base(context)
        {
        }

        public AgreementsRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public AgreementModel JoinAndGet(int id) => JoinAndGetAll(agreement => agreement.Id == id).SingleOrDefault();

        public AgreementModel JoinAndGet(Expression<Func<AgreementModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<AgreementModel> JoinAndGetAll(Expression<Func<AgreementModel, bool>> expression)
        {
            IQueryable<AgreementModel> agreements = expression != null ?
                Context.Agreements.Where(expression) :
                Context.Agreements;

            return agreements
                .Include(agreement => agreement.LeaseModel)
                .ThenInclude(lease => lease.Building)
                .Include(agreement => agreement.LeaserCharacter)
                .Include(agreement => agreement.LeaserGroup);
        }

        public override AgreementModel Get(Func<AgreementModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<AgreementModel> GetAll(Func<AgreementModel, bool> func = null)
        {
            IEnumerable<AgreementModel> agreements = func != null ?
                Context.Agreements.Where(func) :
                Context.Agreements;

            return agreements;
        }
    }
}