using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Vehicle;

namespace VRP.BLL.Services.Interfaces
{
    public interface IVehicleService : IDisposable
    {
        Task<IEnumerable<VehicleDto>> GetAllAsync(Expression<Func<VehicleModel, bool>> expression = null);
        Task<IEnumerable<VehicleDto>> GetAllNoRelatedAsync(Func<VehicleModel, bool> func = null);
        Task<VehicleDto> GetByIdAsync(int id);
        Task<VehicleDto> GetAsync(Func<VehicleModel, bool> func = null);
        Task<VehicleDto> CreateAsync(int creatorId, VehicleDto dto);
        Task<VehicleDto> UpdateAsync(int id, VehicleDto dto);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
    }
}