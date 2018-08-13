using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public GroupRankModel JoinAndGet(Expression<Func<GroupRankModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<GroupRankModel> JoinAndGetAll(Expression<Func<GroupRankModel, bool>> expression = null)
        {
            IQueryable<GroupRankModel> ranks = expression != null ?
                Context.GroupRanks.Where(expression) :
                Context.GroupRanks;

            return ranks
                .Include(rank => rank.Workers);
        }

        public override GroupRankModel Get(Func<GroupRankModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<GroupRankModel> GetAll(Func<GroupRankModel, bool> func = null)
        {
            IEnumerable<GroupRankModel> ranks = func != null ?
                Context.GroupRanks.Where(func) :
                Context.GroupRanks;

            return ranks;
        }
    }
}