/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

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
    public class GroupsRepository : Repository<RoleplayContext, GroupModel>, IJoinableRepository<GroupModel>
    {
        public GroupsRepository(RoleplayContext context) : base(context)
        {
        }

        public GroupModel JoinAndGet(int id) => JoinAndGetAll(group => group.Id == id).SingleOrDefault();

        public async Task<GroupModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public GroupModel JoinAndGet(Expression<Func<GroupModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public async Task<GroupModel> JoinAndGetAsync(Expression<Func<GroupModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        public IEnumerable<GroupModel> JoinAndGetAll(Expression<Func<GroupModel, bool>> expression = null)
        {
            IQueryable<GroupModel> groups = expression != null ?
                Context.Groups.Where(expression) :
                Context.Groups;

            return groups
                .Include(group => group.BossCharacter)
                .Include(group => group.Workers)
                    .ThenInclude(worker => worker.Character)
                .Include(group => group.Workers)
                    .ThenInclude(worker => worker.Character)
                        .ThenInclude(character => character.Account);
        }

        public async Task<IEnumerable<GroupModel>> JoinAndGetAllAsync(Expression<Func<GroupModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }

        public override GroupModel Get(Expression<Func<GroupModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<GroupModel> GetAll(Expression<Func<GroupModel, bool>> expression = null)
        {
            IQueryable<GroupModel> groups = expression != null ?
                Context.Groups.Where(expression) :
                Context.Groups;

            return groups;
        }
    }
}