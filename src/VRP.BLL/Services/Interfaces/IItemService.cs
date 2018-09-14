using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Item;

namespace VRP.BLL.Services.Interfaces
{
    public interface IItemService : IDisposable
    {
        Task<IEnumerable<ItemDto>> GetAllAsync(Expression<Func<ItemModel, bool>> expression = null);
        Task<IEnumerable<ItemDto>> GetAllNoRelatedAsync(Expression<Func<ItemModel, bool>> expression = null);
        Task<ItemDto> GetByIdAsync(int id);
        Task<ItemDto> GetAsync(Expression<Func<ItemModel, bool>> expression = null);
        Task<ItemDto> CreateAsync(int creatorId, ItemDto dto);
        Task<ItemDto> UpdateAsync(int id, ItemDto dto);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
    }
}