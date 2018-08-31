/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Misc;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class ZonesRepository : Repository<RoleplayContext, ZoneModel>
    {
        public ZonesRepository(RoleplayContext context) : base(context)
        {
        }

        public override ZoneModel Get(Expression<Func<ZoneModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<ZoneModel> GetAll(Expression<Func<ZoneModel, bool>> expression = null)
        {
            IQueryable<ZoneModel> zones = expression != null ?
                Context.Zones.Where(expression) :
                Context.Zones;

            return zones;
        }
    }
}