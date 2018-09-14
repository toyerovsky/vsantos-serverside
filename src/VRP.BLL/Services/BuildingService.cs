using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Building;
using VRP.DAL.UnitOfWork;

namespace VRP.BLL.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BuildingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BuildingDto>> GetAllAsync(Expression<Func<BuildingModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<BuildingModel>, BuildingDto[]>(await _unitOfWork.BuildingsRepository.JoinAndGetAllAsync(expression));
        }

        public async Task<IEnumerable<BuildingDto>> GetAllNoRelatedAsync(Expression<Func<BuildingModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<BuildingModel>, BuildingDto[]>(await _unitOfWork.BuildingsRepository.GetAllAsync(expression));
        }

        public async Task<BuildingDto> GetByIdAsync(int id)
        {
            return _mapper.Map<BuildingModel, BuildingDto>(await _unitOfWork.BuildingsRepository.JoinAndGetAsync(id));
        }

        public async Task<BuildingDto> GetAsync(Expression<Func<BuildingModel, bool>> expression)
        {
            return _mapper.Map<BuildingModel, BuildingDto>(await _unitOfWork.BuildingsRepository.JoinAndGetAsync(expression));
        }

        public async Task<BuildingDto> CreateAsync(int creatorId, BuildingDto dto)
        {
            dto.CreatorId = creatorId;
            dto.CreationTime = DateTime.Now;
            dto.Group = null;
            dto.Character = null;
            BuildingModel model = _mapper.Map<BuildingDto, BuildingModel>(dto);
            await _unitOfWork.BuildingsRepository.InsertAsync(model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<BuildingDto> UpdateAsync(int id, BuildingDto dto)
        {
            dto.Character = null;
            dto.Group = null;
            BuildingModel model = await _unitOfWork.BuildingsRepository.GetAsync(id);
            _mapper.Map(dto, model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.BuildingsRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ContainsAsync(int id)
        {
            return await _unitOfWork.BuildingsRepository.ContainsAsync(id);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}