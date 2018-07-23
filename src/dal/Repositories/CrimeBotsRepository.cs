/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.CrimeBot;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class CrimeBotsRepository : Repository<RoleplayContext, CrimeBotModel>, IJoinableRepository<CrimeBotModel>
    {
        public CrimeBotsRepository(RoleplayContext context) : base(context)
        {
        }

        public CrimeBotModel JoinAndGet(int id) => JoinAndGetAll(crimeBot => crimeBot.Id == id).SingleOrDefault();

        public CrimeBotModel JoinAndGet(Expression<Func<CrimeBotModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<CrimeBotModel> JoinAndGetAll(Expression<Func<CrimeBotModel, bool>> expression = null)
        {
            IQueryable<CrimeBotModel> crimeBots = expression != null ?
                Context.CrimeBots.Where(expression) :
                Context.CrimeBots;

            return crimeBots
                .Include(crimeBot => crimeBot.GroupModel)
                    .ThenInclude(group => group.BossCharacter)
                .Include(crimeBot => crimeBot.GroupModel)
                    .ThenInclude(group => group.Workers);
        }

        public override CrimeBotModel Get(Func<CrimeBotModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<CrimeBotModel> GetAll(Func<CrimeBotModel, bool> func = null)
        {
            IEnumerable<CrimeBotModel> crimeBots = func != null ?
                Context.CrimeBots.Where(func) :
                Context.CrimeBots;

            return crimeBots;
        }
    }
}