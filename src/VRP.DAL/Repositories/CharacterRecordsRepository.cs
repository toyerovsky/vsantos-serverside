using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Mdt;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class CharacterRecordsRepository : Repository<RoleplayContext, CharacterRecordModel>, IJoinableRepository<CharacterRecordModel>
    {
        public CharacterRecordsRepository(RoleplayContext context) : base(context)
        {
        }

        public override CharacterRecordModel Get(Expression<Func<CharacterRecordModel, bool>> expression) => GetAll(expression).FirstOrDefault();

        public override IEnumerable<CharacterRecordModel> GetAll(Expression<Func<CharacterRecordModel, bool>> expression = null)
        {
            IEnumerable<CharacterRecordModel> characterRecords = expression != null ?
                Context.CharacterRecords.Where(expression) :
                Context.CharacterRecords;

            return characterRecords;
        }

        public CharacterRecordModel JoinAndGet(int id) =>
            JoinAndGetAll(characterRecord => characterRecord.Id == id).SingleOrDefault();

        public async Task<CharacterRecordModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public CharacterRecordModel JoinAndGet(Expression<Func<CharacterRecordModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public async Task<CharacterRecordModel> JoinAndGetAsync(Expression<Func<CharacterRecordModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

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

        public async Task<IEnumerable<CharacterRecordModel>> JoinAndGetAllAsync(Expression<Func<CharacterRecordModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }
    }
}