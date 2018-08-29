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
    public class VehicleRecordsRepository : Repository<RoleplayContext, VehicleRecordModel>, IJoinableRepository<VehicleRecordModel>
    {
        public VehicleRecordsRepository(RoleplayContext context) : base(context)
        {
        }

        public override VehicleRecordModel Get(Func<VehicleRecordModel, bool> func) => GetAll(func).FirstOrDefault();
        public override async Task<VehicleRecordModel> GetAsync(Func<VehicleRecordModel, bool> func)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<VehicleRecordModel> GetAll(Func<VehicleRecordModel, bool> func = null)
        {
            IEnumerable<VehicleRecordModel> vehicleRecords = func != null ?
                Context.VehicleRecordModels.Where(func) :
                Context.VehicleRecordModels;

            return vehicleRecords;
        }

        public VehicleRecordModel JoinAndGet(int id) =>
            JoinAndGetAll(vehicleRecord => vehicleRecord.Id == id).SingleOrDefault();

        public async Task<VehicleRecordModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }

        public VehicleRecordModel JoinAndGet(Expression<Func<VehicleRecordModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public async Task<VehicleRecordModel> JoinAndGetAsync(Expression<Func<VehicleRecordModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }

        public IEnumerable<VehicleRecordModel> JoinAndGetAll(Expression<Func<VehicleRecordModel, bool>> expression)
        {
            IQueryable<VehicleRecordModel> vehicleRecords = expression != null ?
                Context.VehicleRecordModels.Where(expression) :
                Context.VehicleRecordModels;

            return vehicleRecords
                .Include(vehicleRecord => vehicleRecord.CriminalCases)
                .Include(vehicleRecord => vehicleRecord.Owner);
        }

        public async Task<IEnumerable<VehicleRecordModel>> JoinAndGetAllAsync(Expression<Func<VehicleRecordModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }
    }
}