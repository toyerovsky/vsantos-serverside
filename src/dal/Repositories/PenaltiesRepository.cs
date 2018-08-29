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
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class PenaltiesRepository : Repository<RoleplayContext, PenaltyModel>, IJoinableRepository<PenaltyModel>
    {
        public PenaltiesRepository(RoleplayContext context) : base(context)
        {
        }

        public PenaltyModel JoinAndGet(int id) => JoinAndGetAll(penalty => penalty.Id == id).SingleOrDefault();
        public async Task<PenaltyModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public PenaltyModel JoinAndGet(Expression<Func<PenaltyModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();
        public async Task<PenaltyModel> JoinAndGetAsync(Expression<Func<PenaltyModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        public IEnumerable<PenaltyModel> JoinAndGetAll(Expression<Func<PenaltyModel, bool>> expression = null)
        {
            IQueryable<PenaltyModel> penatlies = expression != null ?
                Context.Penaltlies.Where(expression) :
                Context.Penaltlies;

            return penatlies
                .Include(penatly => penatly.Account)
                .Include(penalty => penalty.Creator)
                .Include(pentaly => pentaly.Character);
        }

        public async Task<IEnumerable<PenaltyModel>> JoinAndGetAllAsync(Expression<Func<PenaltyModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }

        public override PenaltyModel Get(Func<PenaltyModel, bool> func) => GetAll(func).FirstOrDefault();
        public override async Task<PenaltyModel> GetAsync(Func<PenaltyModel, bool> func)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<PenaltyModel> GetAll(Func<PenaltyModel, bool> func = null)
        {
            IEnumerable<PenaltyModel> penatlies = func != null ?
                Context.Penaltlies.Where(func) :
                Context.Penaltlies;

            return penatlies;
        }
    }
}