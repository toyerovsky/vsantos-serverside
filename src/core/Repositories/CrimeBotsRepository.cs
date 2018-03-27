/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models;
using VRP.Core.Interfaces;

namespace VRP.Core.Repositories
{
    public class CrimeBotsRepository : IRepository<CrimeBotModel>
    {
        private readonly RoleplayContext _context;

        public CrimeBotsRepository(RoleplayContext context)
        {
            _context = context ?? throw new ArgumentException(nameof(_context));
        }

        public CrimeBotsRepository() : this(RolePlayContextFactory.NewContext())
        {

        }

        public void Insert(CrimeBotModel model) => _context.CrimeBots.Add(model);

        public bool Contains(CrimeBotModel model)
        {
            return _context.CrimeBots.Any(crimeBot => crimeBot.Id == model.Id);
        }

        public void Update(CrimeBotModel model) => _context.Entry(model).State = EntityState.Modified;

        public void Delete(int id)
        {
            CrimeBotModel crimeBot = _context.CrimeBots.Find(id);
            _context.CrimeBots.Remove(crimeBot);
        }

        public CrimeBotModel Get(int id) => GetAll().Single(c => c.Id == id);

        public CrimeBotModel Get(GroupModel model) => GetAll().Single(c => c.GroupModel.Id == model.Id);

        public IEnumerable<CrimeBotModel> GetAll()
        {
            return _context.CrimeBots.Include(cb => cb.Creator)
                .Include(crimeBot => crimeBot.GroupModel)
                    .ThenInclude(group => group.BossCharacter)
                .Include(crimeBot => crimeBot.GroupModel)
                    .ThenInclude(group => group.Workers)
                .ToList();
        }

        public void Save() => _context.SaveChanges();

        public void Dispose() => _context?.Dispose();
    }
}