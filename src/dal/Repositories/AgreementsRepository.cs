using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Agreement;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class AgreementsRepository : Repository<RoleplayContext, AgreementModel>, IJoinableRepository<AgreementModel>
    {
        public AgreementsRepository(RoleplayContext context) : base(context)
        {
        }

        public AgreementModel JoinAndGet(int id) => JoinAndGetAll(agreement => agreement.Id == id).SingleOrDefault();

        public async Task<AgreementModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public AgreementModel JoinAndGet(Expression<Func<AgreementModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public async Task<AgreementModel> JoinAndGetAsync(Expression<Func<AgreementModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

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

        public async Task<IEnumerable<AgreementModel>> JoinAndGetAllAsync(Expression<Func<AgreementModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }

        public override AgreementModel Get(Expression<Func<AgreementModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<AgreementModel> GetAll(Expression<Func<AgreementModel, bool>> expression = null)
        {
            IEnumerable<AgreementModel> agreements = expression != null ?
                Context.Agreements.Where(expression) :
                Context.Agreements;

            return agreements;
        }
    }
}