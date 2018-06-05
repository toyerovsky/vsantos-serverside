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
using VRP.Core.Database.Models.Vehicle;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class VehiclesRepository : Repository<RoleplayContext, VehicleModel>
    {
        private readonly RoleplayContext _context;

        public VehiclesRepository(RoleplayContext context) : base(context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public VehiclesRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public void Insert(VehicleModel model)
        {
            if ((model.Character?.Id ?? 0) != 0)
                _context.Attach(model.Character);

            foreach (var item in model.ItemsInVehicle)
                if ((item?.Id ?? 0) != 0)
                    _context.Attach(item);

            if ((model.Group?.Id ?? 0) != 0)
                _context.Attach(model.Group);

            _context.Vehicles.Add(model);
        }

        public override VehicleModel Get(int id) => GetAll(vehicle => vehicle.Id == id).SingleOrDefault();

        public override VehicleModel Get(Expression<Func<VehicleModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override VehicleModel GetNoRelated(Expression<Func<VehicleModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public override IEnumerable<VehicleModel> GetAll(Expression<Func<VehicleModel, bool>> expression = null)
        {
            IQueryable<VehicleModel> vehicles = expression != null ?
                _context.Vehicles.Where(expression) :
                _context.Vehicles;

            return vehicles
                .Include(vehicle => vehicle.Character)
                    .ThenInclude(character => character.Account)
                .Include(vehicle => vehicle.Group)
                .Include(vehicle => vehicle.ItemsInVehicle);
        }

        public override IEnumerable<VehicleModel> GetAllNoRelated(Expression<Func<VehicleModel, bool>> expression = null)
        {
            IQueryable<VehicleModel> vehicles = expression != null ?
                _context.Vehicles.Where(expression) :
                _context.Vehicles;

            return vehicles;
        }
    }
}