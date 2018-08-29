using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class GroupRankRepository : Repository<RoleplayContext, GroupRankModel>, IJoinableRepository<GroupRankModel>
    {
        public GroupRankRepository(RoleplayContext context) : base(context)
        {
        }

        public GroupRankModel JoinAndGet(int id) => JoinAndGetAll(account => account.Id == id).SingleOrDefault();
        public async Task<GroupRankModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public GroupRankModel JoinAndGet(Expression<Func<GroupRankModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();
        public async Task<GroupRankModel> JoinAndGetAsync(Expression<Func<GroupRankModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        public IEnumerable<GroupRankModel> JoinAndGetAll(Expression<Func<GroupRankModel, bool>> expression = null)
        {
            IQueryable<GroupRankModel> ranks = expression != null ?
                Context.GroupRanks.Where(expression) :
                Context.GroupRanks;

            return ranks
                .Include(rank => rank.Workers);
        }

        public async Task<IEnumerable<GroupRankModel>> JoinAndGetAllAsync(Expression<Func<GroupRankModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }

        public override GroupRankModel Get(Func<GroupRankModel, bool> func) => GetAll(func).FirstOrDefault();
        public override async Task<GroupRankModel> GetAsync(Func<GroupRankModel, bool> func)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<GroupRankModel> GetAll(Func<GroupRankModel, bool> func = null)
        {
            IEnumerable<GroupRankModel> ranks = func != null ?
                Context.GroupRanks.Where(func) :
                Context.GroupRanks;

            return ranks;
        }
    }
}