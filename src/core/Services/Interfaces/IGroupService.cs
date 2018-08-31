using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Group;

namespace VRP.BLL.Services.Interfaces
{
    public interface IGroupService : IDisposable
    {
        Task<IEnumerable<GroupDto>> GetAllAsync(Expression<Func<GroupModel, bool>> expression = null);
        Task<IEnumerable<GroupDto>> GetAllNoRelatedAsync(Func<GroupModel, bool> func = null);
        Task<GroupDto> GetByIdAsync(int id);
        Task<GroupDto> GetAsync(Func<GroupModel, bool> func = null);
        Task<GroupDto> UpdateAsync(int id, GroupDto dto);
        Task<GroupDto> CreateAsync(int creatorId, GroupDto dto);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
        Task<GroupDto> UpdateImageAsync(int characterId, ImageDto imageDto);
        Task<IEnumerable<GroupDto>> GetByAccountIdAsync(int id);
    }
}