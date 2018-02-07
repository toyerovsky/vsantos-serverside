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
    public class CharactersRepository : IRepository<CharacterModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(CharacterModel model) => Context.Characters.Add(model);

        public bool Contains(CharacterModel model)
        {
            return Context.Characters.Any(character => character.Id == model.Id);
        }

        public void Update(CharacterModel model) => Context.Entry(model).State = EntityState.Modified;

        public void Delete(long id)
        {
            var character = Context.Characters.Find(id);
            Context.Characters.Remove(character);
        }

        public CharacterModel Get(long id) => GetAll().Single(c => c.Id == id);

        public IEnumerable<CharacterModel> GetAll()
        {
            return Context.Characters
                .Include(character => character.Buildings)
                    .ThenInclude(building => building.Items)
                .Include(character => character.Buildings)
                    .ThenInclude(building => building.Creator)
                .Include(character => character.Items)
                    .ThenInclude(item => item.Creator)
                .Include(character => character.Descriptions)
                .Include(character => character.Vehicles)
                    .ThenInclude(vehicle => vehicle.Creator)
                .Include(character => character.Vehicles)
                    .ThenInclude(vehicle => vehicle.ItemsInVehicle)
                        .ThenInclude(item => item.Creator)
                .Include(character => character.Workers)
                    .ThenInclude(group => group.Group)
                .Include(character => character.AccountModel).ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}