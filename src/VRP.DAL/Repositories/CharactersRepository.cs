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
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class CharactersRepository : Repository<RoleplayContext, CharacterModel>, IJoinableRepository<CharacterModel>
    {
        public CharactersRepository(RoleplayContext context) : base(context)
        {
        }

        public CharacterModel JoinAndGet(int id) => JoinAndGetAll(character => character.Id == id).SingleOrDefault();

        public async Task<CharacterModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public CharacterModel JoinAndGet(Expression<Func<CharacterModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public async Task<CharacterModel> JoinAndGetAsync(Expression<Func<CharacterModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

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

        public async Task<IEnumerable<CharacterModel>> JoinAndGetAllAsync(Expression<Func<CharacterModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }

        public override CharacterModel Get(Expression<Func<CharacterModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<CharacterModel> GetAll(Expression<Func<CharacterModel, bool>> expression = null)
        {
            IEnumerable<CharacterModel> characters = expression != null ?
                Context.Characters.Where(expression) :
                Context.Characters;

            return characters;
        }
    }
}