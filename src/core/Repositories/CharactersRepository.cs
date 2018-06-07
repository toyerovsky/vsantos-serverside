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
using VRP.Core.Database.Models.Character;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class CharactersRepository : Repository<RoleplayContext, CharacterModel>
    {
        public CharactersRepository(RoleplayContext context) : base(context)
        {
        }

        public CharactersRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override void Insert(CharacterModel model)
        {
            foreach (var vehicle in model.Vehicles)
                if ((vehicle?.Id ?? 0) != 0)
                    Context.Attach(vehicle);

            foreach (var item in model.Items)
                if ((item?.Id ?? 0) != 0)
                    Context.Attach(item);

            if ((model.Account?.Id ?? 0) != 0)
                Context.Attach(model.Account);

            foreach (var building in model.Buildings)
                if ((building?.Id ?? 0) != 0)
                    Context.Attach(building);

            foreach (var description in model.Descriptions)
                if ((description?.Id ?? 0) != 0)
                    Context.Attach(description);

            foreach (var worker in model.Workers)
                if ((worker?.Id ?? 0) != 0)
                    Context.Attach(worker);

            Context.Characters.Add(model);
        }

        public CharacterModel JoinAndGet(int id) => JoinAndGetAll(character => character.Id == id).SingleOrDefault();

        public CharacterModel JoinAndGet(Expression<Func<CharacterModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<CharacterModel> JoinAndGetAll(Expression<Func<CharacterModel, bool>> expression = null)
        {
            IQueryable<CharacterModel> characters = expression != null ?
                Context.Characters.Where(expression) :
                Context.Characters;

            return characters
                .Include(character => character.Buildings)
                    .ThenInclude(building => building.ItemsInBuilding)
                .Include(character => character.Buildings)
                .Include(character => character.Items)
                .Include(character => character.Descriptions)
                .Include(character => character.Vehicles)
                .Include(character => character.Vehicles)
                    .ThenInclude(vehicle => vehicle.ItemsInVehicle)
                .Include(character => character.Workers)
                    .ThenInclude(group => group.Group)
                .Include(character => character.Account);
        }

        public override CharacterModel Get(Func<CharacterModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<CharacterModel> GetAll(Func<CharacterModel, bool> func = null)
        {
            IEnumerable<CharacterModel> characters = func != null ?
                Context.Characters.Where(func) :
                Context.Characters;

            return characters;
        }
    }
}