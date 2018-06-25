using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public override IEnumerable<VehicleRecordModel> GetAll(Func<VehicleRecordModel, bool> func = null)
        {
            IEnumerable<VehicleRecordModel> vehicleRecords = func != null ?
                Context.VehicleRecordModels.Where(func) :
                Context.VehicleRecordModels;

            return vehicleRecords;
        }

        public VehicleRecordModel JoinAndGet(int id) =>
            JoinAndGetAll(vehicleRecord => vehicleRecord.Id == id).SingleOrDefault();

        public VehicleRecordModel JoinAndGet(Expression<Func<VehicleRecordModel, bool>> expression) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<VehicleRecordModel> JoinAndGetAll(Expression<Func<VehicleRecordModel, bool>> expression)
        {
            IQueryable<VehicleRecordModel> vehicleRecords = expression != null ?
                Context.VehicleRecordModels.Where(expression) :
                Context.VehicleRecordModels;

            return vehicleRecords
                .Include(vehicleRecord => vehicleRecord.CriminalCases)
                .Include(vehicleRecord => vehicleRecord.Owner);
        }
    }
}