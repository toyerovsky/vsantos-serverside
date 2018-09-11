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
    public class ItemTemplateService : IItemTemplateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemTemplateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemTemplateDto>> GetAllAsync(Expression<Func<ItemTemplateModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<ItemTemplateModel>, ItemTemplateDto[]>(await _unitOfWork.ItemTemplatesRepository.GetAllAsync(expression));
        }

        public async Task<IEnumerable<ItemTemplateDto>> GetAllNoRelatedAsync(Expression<Func<ItemTemplateModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<ItemTemplateModel>, ItemTemplateDto[]>(await _unitOfWork.ItemTemplatesRepository.GetAllAsync(expression));
        }

        public async Task<ItemTemplateDto> GetByIdAsync(int id)
        {
            return _mapper.Map<ItemTemplateModel, ItemTemplateDto>(await _unitOfWork.ItemTemplatesRepository.GetAsync(id));
        }

        public async Task<ItemTemplateDto> GetAsync(Expression<Func<ItemTemplateModel, bool>> expression)
        {
            return _mapper.Map<ItemTemplateModel, ItemTemplateDto>(await _unitOfWork.ItemTemplatesRepository.GetAsync(expression));
        }

        public async Task<ItemTemplateDto> CreateAsync(int creatorId, ItemTemplateDto dto)
        {
            dto.CreatorId = creatorId;
            dto.CreationTime = DateTime.Now;
            ItemTemplateModel model = _mapper.Map<ItemTemplateDto, ItemTemplateModel>(dto);
            await _unitOfWork.ItemTemplatesRepository.InsertAsync(model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<ItemTemplateDto> UpdateAsync(int id, ItemTemplateDto dto)
        {
            ItemTemplateModel model = await _unitOfWork.ItemTemplatesRepository.GetAsync(id);
            _mapper.Map(dto, model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.ItemTemplatesRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ContainsAsync(int id)
        {
            return await _unitOfWork.ItemTemplatesRepository.ContainsAsync(id);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}