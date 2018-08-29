/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public override ZoneModel Get(Func<ZoneModel, bool> func) => GetAll(func).FirstOrDefault();
        public override async Task<ZoneModel> GetAsync(Func<ZoneModel, bool> func)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ZoneModel> GetAll(Func<ZoneModel, bool> func = null)
        {
            IEnumerable<ZoneModel> zones = func != null ?
                Context.Zones.Where(func) :
                Context.Zones;

            return zones;
        }
    }
}