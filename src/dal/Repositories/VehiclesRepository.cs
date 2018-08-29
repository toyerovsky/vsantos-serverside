/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class VehiclesRepository : Repository<RoleplayContext, VehicleModel>, IJoinableRepository<VehicleModel>
    {
        public VehiclesRepository(RoleplayContext context) : base(context)
        {
        }

        public VehicleModel JoinAndGet(int id) => JoinAndGetAll(vehicle => vehicle.Id == id).SingleOrDefault();
        public async Task<VehicleModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public VehicleModel JoinAndGet(Expression<Func<VehicleModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();
        public async Task<VehicleModel> JoinAndGetAsync(Expression<Func<VehicleModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

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

        public async Task<IEnumerable<VehicleModel>> JoinAndGetAllAsync(Expression<Func<VehicleModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
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