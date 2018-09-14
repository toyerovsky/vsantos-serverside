using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Group;

namespace VRP.BLL.Services.Interfaces
{
    public interface IWorkerService : IDisposable
    {
        Task<IEnumerable<WorkerDto>> GetAllAsync(Expression<Func<WorkerModel, bool>> expression = null);
        Task<IEnumerable<WorkerDto>> GetAllNoRelatedAsync(Expression<Func<WorkerModel, bool>> expression = null);
        Task<WorkerDto> GetByIdAsync(int id);
        Task<WorkerDto> GetAsync(Expression<Func<WorkerModel, bool>> expression = null);
        Task<IEnumerable<WorkerDto>> GetByGroupIdAsync(int id);
        Task<WorkerDto> CreateAsync(WorkerDto dto);
        Task<WorkerDto> UpdateAsync(int id, WorkerDto dto);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
    }
}