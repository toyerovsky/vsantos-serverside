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

        public ZoneModel Get(int id) => GetAll(g => g.Id == id).SingleOrDefault();

        public IEnumerable<ZoneModel> GetAll(Expression<Func<ZoneModel, bool>> expression = null)
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