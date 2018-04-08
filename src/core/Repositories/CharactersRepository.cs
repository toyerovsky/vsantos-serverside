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

        public CharactersRepository() : this(RolePlayContextFactory.NewContext())
        {
        }

        public void Insert(CharacterModel model) => _context.Characters.Add(model);

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

        public CharacterModel Get(int id) => GetAll(c => c.Id == id).Single();

        public IEnumerable<CharacterModel> GetAll(Expression<Func<CharacterModel, bool>> expression = null)
        {
            IQueryable<CharacterModel> characters = expression != null ?
                _context.Characters.Where(expression) :
                _context.Characters;

            return characters
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
                .Include(character => character.Account);
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();
    }
}