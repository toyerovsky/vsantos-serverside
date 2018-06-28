﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Building;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class BuildingsRepository : Repository<RoleplayContext, BuildingModel>, IJoinableRepository<BuildingModel>
    {
        public BuildingsRepository(RoleplayContext context) : base(context)
        {
        }

        public override void Insert(BuildingModel model)
        {
            if ((model.Character?.Id ?? 0) != 0)
                Context.Attach(model.Character);

            foreach (var item in model.ItemsInBuilding)
                if ((item?.Id ?? 0) != 0)
                    Context.Attach(item);

            if ((model.Group?.Id ?? 0) != 0)
                Context.Attach(model.Group);

            Context.Buildings.Add(model);
        }

        public BuildingModel JoinAndGet(int id) => JoinAndGetAll(building => building.Id == id).SingleOrDefault();

        public BuildingModel JoinAndGet(Expression<Func<BuildingModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public override BuildingModel Get(Func<BuildingModel, bool> func) => GetAll(func).FirstOrDefault();

        public IEnumerable<BuildingModel> JoinAndGetAll(Expression<Func<BuildingModel, bool>> expression = null)
        {
            IQueryable<BuildingModel> buildings = expression != null ?
                Context.Buildings.Where(expression) :
                Context.Buildings;

            return buildings
                .Include(building => building.Character)
                .Include(building => building.Group)
                .Include(building => building.ItemsInBuilding);
        }

        public override IEnumerable<BuildingModel> GetAll(Func<BuildingModel, bool> func = null)
        {
            IEnumerable<BuildingModel> buildings = func != null ?
                Context.Buildings.Where(func) :
                Context.Buildings;

            return buildings;
        }
    }
}