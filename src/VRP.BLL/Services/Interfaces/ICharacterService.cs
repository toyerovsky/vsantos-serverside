using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Character;

namespace VRP.BLL.Services.Interfaces
{
    public interface ICharacterService : IDisposable
    {
        Task<IEnumerable<CharacterDto>> GetAllAsync(Expression<Func<CharacterModel, bool>> expression = null);
        Task<IEnumerable<CharacterDto>> GetAllNoRelatedAsync(Expression<Func<CharacterModel, bool>> expression = null);
        Task<CharacterDto> GetByIdAsync(int id);
        Task<CharacterDto> GetAsync(Expression<Func<CharacterModel, bool>> expression = null);
        Task<CharacterDto> UpdateAsync(int id, CharacterDto dto);
        Task<CharacterDto> CreateAsync(CharacterDto dto);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
        Task<CharacterDto> UpdateImageAsync(int characterId, ImageDto imageDto);
    }
}