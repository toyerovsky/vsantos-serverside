/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
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
    public class VehiclesRepository : IRepository<VehicleModel>
    {
        private readonly RoleplayContext _context;

        public VehiclesRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public VehiclesRepository() : this(RolePlayContextFactory.NewContext())
        {
        }

        public void Insert(VehicleModel model) => _context.Vehicles.Add(model);

        public bool Contains(VehicleModel model)
        {
            return _context.Vehicles.Any(vehicle => vehicle.Id == model.Id);
        }

        public void Update(VehicleModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            VehicleModel vehicle = _context.Vehicles.Find(id);
            _context.Vehicles.Remove(vehicle);
        }

        public VehicleModel Get(int id) => GetAll(v => v.Id == id).Single();

        public IEnumerable<VehicleModel> GetAll(Expression<Func<VehicleModel, bool>> expression = null)
        {
            IQueryable<VehicleModel> vehicles = expression != null ?
                _context.Vehicles.Where(expression) :
                _context.Vehicles;

            return vehicles
                .Include(vehicle => vehicle.Creator)
                .Include(vehicle => vehicle.Character)
                    .ThenInclude(character => character.Account)
                .Include(vehicle => vehicle.Group)
                .Include(vehicle => vehicle.ItemsInVehicle)
                    .ThenInclude(item => item.Creator);
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}