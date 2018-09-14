using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Group;

namespace VRP.BLL.Services.Interfaces
{
    public interface IGroupRankService : IDisposable
    {
        Task<IEnumerable<GroupRankDto>> GetAllAsync(Expression<Func<GroupRankModel, bool>> expression = null);
        Task<IEnumerable<GroupRankDto>> GetAllNoRelatedAsync(Expression<Func<GroupRankModel, bool>> expression = null);
        Task<GroupRankDto> GetByIdAsync(int id);
        Task<GroupRankDto> GetAsync(Expression<Func<GroupRankModel, bool>> expression = null);
        Task<GroupRankDto> UpdateAsync(int id, GroupRankDto dto);
        Task<GroupRankDto> CreateAsync(int creatorId, GroupRankDto dto);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
        Task<IEnumerable<GroupRankDto>> GetByGroupIdAsync(int id);
    }
}