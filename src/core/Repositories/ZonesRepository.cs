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
using VRP.Core.Database;
using VRP.Core.Database.Models.Misc;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class ZonesRepository : Repository<RoleplayContext, ZoneModel>
    {
        private readonly RoleplayContext _context;

        public ZonesRepository(RoleplayContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public ZonesRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override ZoneModel Get(int id) => GetAll(zone => zone.Id == id).SingleOrDefault();

        public override ZoneModel Get(Expression<Func<ZoneModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override ZoneModel GetNoRelated(Expression<Func<ZoneModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public override IEnumerable<ZoneModel> GetAll(Expression<Func<ZoneModel, bool>> expression = null)
        {
            IQueryable<ZoneModel> zones = expression != null ?
                _context.Zones.Where(expression) :
                _context.Zones;

            return zones;
        }

        public override IEnumerable<ZoneModel> GetAllNoRelated(Expression<Func<ZoneModel, bool>> expression = null)
        {
            IQueryable<ZoneModel> zones = expression != null ?
                _context.Zones.Where(expression) :
                _context.Zones;

            return zones;
        }
    }
}