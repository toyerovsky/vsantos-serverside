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
using VRP.Core.Database.Models;
using VRP.Core.Interfaces;

namespace VRP.Core.Repositories
{
    public class ZonesRepository : IRepository<ZoneModel>
    {
        private readonly RoleplayContext _context;

        public ZonesRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public ZonesRepository() : this(RolePlayContextFactory.NewContext())
        {
        }

        public void Insert(ZoneModel model)
        {
            _context.Zones.Add(model);
        }

        public bool Contains(ZoneModel model)
        {
            return _context.Zones.Any(zone => zone.Id == model.Id);
        }

        public void Update(ZoneModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            ZoneModel zone = _context.Zones.Find(id);
            _context.Zones.Remove(zone);
        }

        public ZoneModel Get(int id) => GetAll(zone => zone.Id == id).SingleOrDefault();

        public ZoneModel GetNoRelated(int id)
        {
            ZoneModel zone = _context.Zones.Find(id);
            return zone;
        }

        public ZoneModel Get(Expression<Func<ZoneModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public ZoneModel GetNoRelated(Expression<Func<ZoneModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public IEnumerable<ZoneModel> GetAll(Expression<Func<ZoneModel, bool>> expression = null)
        {
            IQueryable<ZoneModel> zones = expression != null ?
                _context.Zones.Where(expression) :
                _context.Zones;

            return zones;
        }

        public IEnumerable<ZoneModel> GetAllNoRelated(Expression<Func<ZoneModel, bool>> expression = null)
        {
            IQueryable<ZoneModel> zones = expression != null ?
                _context.Zones.Where(expression) :
                _context.Zones;

            return zones;
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}