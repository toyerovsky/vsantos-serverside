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
using VRP.Core.Database.Models.Building;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class BuildingsRepository : Repository<RoleplayContext, BuildingModel>
    {
        private readonly RoleplayContext _context;

        public BuildingsRepository(RoleplayContext context) : base(context)
        {
        }

        public BuildingsRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override void Insert(BuildingModel model)
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


        public override BuildingModel Get(int id) => GetAll(building => building.Id == id).SingleOrDefault();

        public override BuildingModel Get(Expression<Func<BuildingModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override BuildingModel GetNoRelated(Expression<Func<BuildingModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public override IEnumerable<BuildingModel> GetAll(Expression<Func<BuildingModel, bool>> expression = null)
        {
            IQueryable<BuildingModel> buildings = expression != null ?
                _context.Buildings.Where(expression) :
                _context.Buildings;

            return buildings
                .Include(building => building.Character)
                .Include(building => building.Group)
                .Include(building => building.ItemsInBuilding);
        }

        public override IEnumerable<BuildingModel> GetAllNoRelated(Expression<Func<BuildingModel, bool>> expression = null)
        {
            IQueryable<BuildingModel> buildings = expression != null ?
                _context.Buildings.Where(expression) :
                _context.Buildings;

            return buildings;
        }
    }
}