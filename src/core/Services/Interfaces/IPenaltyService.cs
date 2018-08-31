using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Account;

namespace VRP.BLL.Services.Interfaces
{
    public interface IPenaltyService : IDisposable
    {
        Task<IEnumerable<PenaltyDto>> GetAllAsync(Expression<Func<PenaltyModel, bool>> expression = null);
        Task<IEnumerable<PenaltyDto>> GetAllNoRelatedAsync(Func<PenaltyModel, bool> func = null);
        Task<PenaltyDto> GetByIdAsync(int id);
        Task<PenaltyDto> GetAsync(Func<PenaltyModel, bool> func = null);
        Task<PenaltyDto> CreateAsync(int creatorId, PenaltyDto dto);
        Task<PenaltyDto> UpdateAsync(int id, PenaltyDto dto);
        Task<PenaltyDto> DeactivateAsync(int deactivatorId, int penaltyId);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
    }
}