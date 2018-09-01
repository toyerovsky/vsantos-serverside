using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Item;

namespace VRP.BLL.Services.Interfaces
{
    public interface IItemTemplateService
    {
        Task<IEnumerable<ItemTemplateDto>> GetAllNoRelatedAsync(Expression<Func<ItemTemplateModel, bool>> expression = null);
        Task<ItemTemplateDto> GetByIdAsync(int id);
        Task<ItemTemplateDto> GetAsync(Expression<Func<ItemTemplateModel, bool>> expression = null);
        Task<ItemTemplateDto> CreateAsync(int creatorId, ItemTemplateDto dto);
        Task<ItemTemplateDto> UpdateAsync(int id, ItemTemplateDto dto);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
    }
}