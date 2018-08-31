using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Building;

namespace VRP.BLL.Services.Interfaces
{
    public interface IBuildingService : IDisposable
    {
        Task<IEnumerable<BuildingDto>> GetAllAsync(Expression<Func<BuildingModel, bool>> expression = null);
        Task<IEnumerable<BuildingDto>> GetAllNoRelatedAsync(Expression<Func<BuildingModel, bool>> expression = null);
        Task<BuildingDto> GetByIdAsync(int id);
        Task<BuildingDto> GetAsync(Expression<Func<BuildingModel, bool>> expression = null);
        Task<BuildingDto> UpdateAsync(int id, BuildingDto dto);
        Task<BuildingDto> CreateAsync(int creatorId, BuildingDto dto);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
    }
}