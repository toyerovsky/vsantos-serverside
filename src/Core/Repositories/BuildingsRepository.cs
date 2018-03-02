/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serverside.Core.Database;
using Serverside.Core.Database.Models;
using Serverside.Core.Interfaces;

namespace Serverside.Core.Repositories
{
    public class BuildingsRepository : IRepository<BuildingModel>
    {
        private readonly RoleplayContext _context;

        public BuildingsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public BuildingsRepository()
        {
            _context = RolePlayContextFactory.NewContext();
        }

        public void Insert(BuildingModel model) => _context.Buildings.Add(model);

        public bool Contains(BuildingModel model)
        {
            return _context.Buildings.Any(building => building.Id == model.Id);
        }

        public void Update(BuildingModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var building = _context.Buildings.Find(id);
            _context.Buildings.Remove(building);
        }

        public BuildingModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<BuildingModel> GetAll()
        {
            return _context.Buildings
                .Include(building => building.Creator)
                .Include(building => building.Character)
                .Include(building => building.Group)
                .Include(building => building.Items)
                    .ThenInclude(item => item.Creator)
                .ToList();
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}