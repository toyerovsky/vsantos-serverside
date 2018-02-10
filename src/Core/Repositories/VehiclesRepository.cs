/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serverside.Core.Database;
using Serverside.Core.Database.Models;
using Serverside.Core.Interfaces;

namespace Serverside.Core.Repositories
{
    public class VehiclesRepository : IRepository<VehicleModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(VehicleModel model) => Context.Vehicles.Add(model);

        public bool Contains(VehicleModel model)
        {
            return Context.Vehicles.Any(vehicle => vehicle.Id == model.Id);
        }

        public void Update(VehicleModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var vehicle = Context.Vehicles.Find(id);
            Context.Vehicles.Remove(vehicle);
        }

        public VehicleModel Get(long id) => Context.Vehicles.Find(id);

        public IEnumerable<VehicleModel> GetAll()
        {
            return Context.Vehicles
                .Include(vehicle => vehicle.Creator)
                .Include(vehicle => vehicle.Character)
                    .ThenInclude(character => character.AccountModel)
                .Include(vehicle => vehicle.Group)
                .Include(vehicle => vehicle.ItemsInVehicle)
                    .ThenInclude(item => item.Creator).ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}