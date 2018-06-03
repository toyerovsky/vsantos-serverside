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
    public class BuildingsRepository : IRepository<BuildingModel>
    {
        private readonly RoleplayContext _context;

        public BuildingsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public BuildingsRepository() : this(RolePlayContextFactory.NewContext())
        {
        }

        public void Insert(BuildingModel model)
        {
            if ((model.Character?.Id ?? 0) != 0)
                _context.Attach(model.Character);

            foreach (var item in model.ItemsInBuilding)
                if ((item?.Id ?? 0) != 0)
                    _context.Attach(item);

            if ((model.Group?.Id ?? 0) != 0)
                _context.Attach(model.Group);

            _context.Buildings.Add(model);
        }

        public bool Contains(BuildingModel model)
        {
            return _context.Buildings.Any(building => building.Id == model.Id);
        }

        public void Update(BuildingModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            BuildingModel building = _context.Buildings.Find(id);
            _context.Buildings.Remove(building);
        }

        public BuildingModel Get(int id) => GetAll(building => building.Id == id).SingleOrDefault();

        public BuildingModel GetNoRelated(int id)
        {
            BuildingModel building = _context.Buildings.Find(id);
            return building;
        }

        public BuildingModel Get(Expression<Func<BuildingModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public BuildingModel GetNoRelated(Expression<Func<BuildingModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public IEnumerable<BuildingModel> GetAll(Expression<Func<BuildingModel, bool>> expression = null)
        {
            IQueryable<BuildingModel> buildings = expression != null ?
                _context.Buildings.Where(expression) :
                _context.Buildings;

            return buildings
                .Include(building => building.Character)
                .Include(building => building.Group)
                .Include(building => building.ItemsInBuilding);
        }

        public IEnumerable<BuildingModel> GetAllNoRelated(Expression<Func<BuildingModel, bool>> expression = null)
        {
            IQueryable<BuildingModel> buildings = expression != null ?
                _context.Buildings.Where(expression) :
                _context.Buildings;

            return buildings;
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}