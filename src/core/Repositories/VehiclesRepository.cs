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
using VRP.Core.Interfaces;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class VehiclesRepository : Repository<RoleplayContext, VehicleModel>, IJoinableRepository<VehicleModel>
    {
        public VehiclesRepository(RoleplayContext context) : base(context)
        {
        }

        public VehiclesRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public void InsertWithRelated(VehicleModel model)
        {
            if ((model.Character?.Id ?? 0) != 0)
                Context.Attach(model.Character);

            foreach (var item in model.ItemsInVehicle)
                if ((item?.Id ?? 0) != 0)
                    Context.Attach(item);

            if ((model.Group?.Id ?? 0) != 0)
                Context.Attach(model.Group);

            Context.Vehicles.Add(model);
        }

        public VehicleModel JoinAndGet(int id) => JoinAndGetAll(vehicle => vehicle.Id == id).SingleOrDefault();

        public VehicleModel JoinAndGet(Expression<Func<VehicleModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<VehicleModel> JoinAndGetAll(Expression<Func<VehicleModel, bool>> expression = null)
        {
            IQueryable<VehicleModel> vehicles = expression != null ?
                Context.Vehicles.Where(expression) :
                Context.Vehicles;

            return vehicles
                .Include(vehicle => vehicle.Character)
                    .ThenInclude(character => character.Account)
                .Include(vehicle => vehicle.Group)
                .Include(vehicle => vehicle.ItemsInVehicle);
        }

        public override VehicleModel Get(Func<VehicleModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<VehicleModel> GetAll(Func<VehicleModel, bool> func = null)
        {
            IEnumerable<VehicleModel> vehicles = func != null ?
                Context.Vehicles.Where(func) :
                Context.Vehicles;

            return vehicles;
        }
    }
}