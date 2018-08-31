using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
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

        public async Task<IEnumerable<CharacterDto>> GetAllNoRelatedAsync(Func<CharacterModel, bool> func)
        {
            return _mapper.Map<IEnumerable<CharacterModel>, CharacterDto[]>(await _unitOfWork.CharactersRepository.GetAllAsync(func));
        }

        public async Task<CharacterDto> GetByIdAsync(int id)
        {
            return _mapper.Map<CharacterModel, CharacterDto>(await _unitOfWork.CharactersRepository.GetAsync(id));
        }

        public async Task<CharacterDto> GetAsync(Func<CharacterModel, bool> func)
        {
            return _mapper.Map<CharacterModel, CharacterDto>(await _unitOfWork.CharactersRepository.GetAsync(func));
        }

        public async Task<CharacterDto> CreateAsync(CharacterDto dto)
        {
            dto.IsAlive = true;
            dto.CreateTime = DateTime.Now;
            dto.Money = 2000;
            CharacterModel model = _mapper.Map<CharacterDto, CharacterModel>(dto);
            await _unitOfWork.CharactersRepository.InsertAsync(model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<CharacterDto> UpdateAsync(int id, CharacterDto dto)
        {
            CharacterModel model = await _unitOfWork.CharactersRepository.JoinAndGetAsync(id);
            _unitOfWork.CharactersRepository.BeginUpdate(model);
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
            CharacterModel characterModel = _unitOfWork.CharactersRepository.Get(characterId);
            _unitOfWork.CharactersRepository.BeginUpdate(characterModel);
            characterModel.ImageUploadDate = DateTime.Now;
            characterModel.ImageUrl = await imageTask;
            await _unitOfWork.SaveAsync();
            return Mapper.Map<CharacterModel, CharacterDto>(characterModel);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}