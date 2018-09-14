using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.UnitOfWork;

namespace VRP.BLL.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync(Expression<Func<ItemModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<ItemModel>, ItemDto[]>(await _unitOfWork.ItemsRepository.JoinAndGetAllAsync(expression));
        }

        public async Task<IEnumerable<ItemDto>> GetAllNoRelatedAsync(Expression<Func<ItemModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<ItemModel>, ItemDto[]>(await _unitOfWork.ItemsRepository.GetAllAsync(expression));
        }

        public async Task<ItemDto> GetByIdAsync(int id)
        {
            return _mapper.Map<ItemModel, ItemDto>(await _unitOfWork.ItemsRepository.JoinAndGetAsync(id));
        }

        public async Task<ItemDto> GetAsync(Expression<Func<ItemModel, bool>> expression)
        {
            return _mapper.Map<ItemModel, ItemDto>(await _unitOfWork.ItemsRepository.JoinAndGetAsync(expression));
        }

        public async Task<ItemDto> CreateAsync(int creatorId, ItemDto dto)
        {
            dto.CreatorId = creatorId;
            dto.CreationTime = DateTime.Now;
            ItemModel model = _mapper.Map<ItemDto, ItemModel>(dto);
            await _unitOfWork.ItemsRepository.InsertAsync(model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<ItemDto> UpdateAsync(int id, ItemDto dto)
        {
            dto.Character = null;
            dto.Vehicle = null;
            dto.Building = null;
            dto.TuningInVehicle = null;
            ItemModel model = await _unitOfWork.ItemsRepository.GetAsync(id);
            _mapper.Map(dto, model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.ItemsRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ContainsAsync(int id)
        {
            return await _unitOfWork.ItemsRepository.ContainsAsync(id);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}