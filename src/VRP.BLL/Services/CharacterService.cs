using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.UnitOfWork;

namespace VRP.BLL.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public CharacterService(IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CharacterDto>> GetAllAsync(Expression<Func<CharacterModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<CharacterModel>, CharacterDto[]>(await _unitOfWork.CharactersRepository.JoinAndGetAllAsync(expression));
        }

        public async Task<IEnumerable<CharacterDto>> GetAllNoRelatedAsync(Expression<Func<CharacterModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<CharacterModel>, CharacterDto[]>(await _unitOfWork.CharactersRepository.GetAllAsync(expression));
        }

        public async Task<CharacterDto> GetByIdAsync(int id)
        {
            return _mapper.Map<CharacterModel, CharacterDto>(await _unitOfWork.CharactersRepository.JoinAndGetAsync(id));
        }

        public async Task<CharacterDto> GetAsync(Expression<Func<CharacterModel, bool>> expression)
        {
            return _mapper.Map<CharacterModel, CharacterDto>(await _unitOfWork.CharactersRepository.JoinAndGetAsync(expression));
        }

        public async Task<CharacterDto> CreateAsync(CharacterDto dto)
        {
            dto.IsAlive = true;
            dto.CreateTime = DateTime.Now;
            dto.Money = 2000;
            dto.Account = null;
            CharacterModel model = _mapper.Map<CharacterDto, CharacterModel>(dto);
            await _unitOfWork.CharactersRepository.InsertAsync(model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<CharacterDto> UpdateAsync(int id, CharacterDto dto)
        {
            dto.Account = null;
            CharacterModel model = await _unitOfWork.CharactersRepository.GetAsync(id);
            _mapper.Map(dto, model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.CharactersRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ContainsAsync(int id)
        {
            return await _unitOfWork.CharactersRepository.ContainsAsync(id);
        }

        public async Task<CharacterDto> UpdateImageAsync(int characterId, ImageDto imageDto)
        {
            var imageTask = _imageService.UploadImageAsync(imageDto);
            CharacterModel characterModel = await _unitOfWork.CharactersRepository.GetAsync(characterId);
            characterModel.ImageUploadDate = DateTime.Now;
            characterModel.ImageUrl = await imageTask;
            await _unitOfWork.SaveAsync();
            return _mapper.Map<CharacterModel, CharacterDto>(characterModel);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}