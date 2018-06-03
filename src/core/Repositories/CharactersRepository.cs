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
using VRP.Core.Database.Models;
using VRP.Core.Interfaces;

namespace VRP.Core.Repositories
{
    public class CharactersRepository : IRepository<CharacterModel>
    {
        private readonly RoleplayContext _context;

        public CharactersRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public CharactersRepository() : this(RoleplayContextFactory.NewContext())
        {
        }

        public void Insert(CharacterModel model)
        {
            foreach (var vehicle in model.Vehicles)
                if ((vehicle?.Id ?? 0) != 0)
                    _context.Attach(vehicle);

            foreach (var item in model.Items)
                if ((item?.Id ?? 0) != 0)
                    _context.Attach(item);

            if ((model.Account?.Id ?? 0) != 0)
                _context.Attach(model.Account);

            foreach (var building in model.Buildings)
                if ((building?.Id ?? 0) != 0)
                    _context.Attach(building);

            foreach (var description in model.Descriptions)
                if ((description?.Id ?? 0) != 0)
                    _context.Attach(description);

            foreach (var worker in model.Workers)
                if ((worker?.Id ?? 0) != 0)
                    _context.Attach(worker);

            _context.Characters.Add(model);
        }

        public bool Contains(CharacterModel model)
        {
            return _context.Characters.Any(character => character.Id == model.Id);
        }

        public void Update(CharacterModel model)
        {
            _context.Entry(model).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            CharacterModel character = _context.Characters.Find(id);
            _context.Characters.Remove(character);
        }

        public CharacterModel Get(int id) => GetAll(character => character.Id == id).SingleOrDefault();

        public CharacterModel GetNoRelated(int id)
        {
            CharacterModel character = _context.Characters.Find(id);
            return character;
        }

        public CharacterModel Get(Expression<Func<CharacterModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public CharacterModel GetNoRelated(Expression<Func<CharacterModel, bool>> expression) => GetAllNoRelated(expression).FirstOrDefault();

        public IEnumerable<CharacterModel> GetAll(Expression<Func<CharacterModel, bool>> expression = null)
        {
            IQueryable<CharacterModel> characters = expression != null ?
                _context.Characters.Where(expression) :
                _context.Characters;

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

        public IEnumerable<CharacterModel> GetAllNoRelated(Expression<Func<CharacterModel, bool>> expression = null)
        {
            IQueryable<CharacterModel> characters = expression != null ?
                _context.Characters.Where(expression) :
                _context.Characters;

            return characters;
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();
    }
}