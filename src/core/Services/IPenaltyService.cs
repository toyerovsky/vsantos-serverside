using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.DAL.Database.Models.Account;
using VRP.vAPI.Dto;

namespace VRP.BLL.Services
{
    public interface IPenaltyService : IDisposable
    {
        Task<IEnumerable<PenaltyDto>> GetAllAsync(Expression<Func<PenaltyModel, bool>> expression);
        Task<IEnumerable<PenaltyDto>> GetAllNoRelatedAsync(Func<PenaltyModel, bool> func);
        Task<PenaltyDto> GetByIdAsync(int id);
        Task<PenaltyDto> CreateAsync(int creatorId, PenaltyDto dto);
        Task<PenaltyDto> UpdateAsync(int id, PenaltyDto dto);
        Task<PenaltyDto> DeactivateAsync(int deactivatorId, int penaltyId);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
    }
}