using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models.Mdt;
using VRP.Core.Interfaces;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class CharacterRecordsRepository : Repository<RoleplayContext, CharacterRecordModel>, IJoinableRepository<CharacterRecordModel>
    {
        public CharacterRecordsRepository(RoleplayContext context) : base(context)
        {
        }

        public CharacterRecordsRepository() : this(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override CharacterRecordModel Get(Func<CharacterRecordModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<CharacterRecordModel> GetAll(Func<CharacterRecordModel, bool> func = null)
        {
            IEnumerable<CharacterRecordModel> characterRecords = func != null ?
                Context.CharacterRecords.Where(func) :
                Context.CharacterRecords;

            return characterRecords;
        }

        public CharacterRecordModel JoinAndGet(int id) =>
            JoinAndGetAll(characterRecord => characterRecord.Id == id).SingleOrDefault();

        public CharacterRecordModel JoinAndGet(Expression<Func<CharacterRecordModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<CharacterRecordModel> JoinAndGetAll(Expression<Func<CharacterRecordModel, bool>> expression)
        {
            IQueryable<CharacterRecordModel> characterRecords = expression != null ?
                Context.CharacterRecords.Where(expression) :
                Context.CharacterRecords;

            return characterRecords
                .Include(characterRecord => characterRecord.Vehicles)
                    .ThenInclude(vehicleRecord => vehicleRecord.CriminalCases)
                .Include(characterRecord => characterRecord.CriminalCases);
        }
    }
}