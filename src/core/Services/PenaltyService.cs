using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.UnitOfWork;

namespace VRP.BLL.Services
{
    public class PenaltyService : IPenaltyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PenaltyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PenaltyDto>> GetAllAsync(Expression<Func<PenaltyModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<PenaltyModel>, PenaltyDto[]>(await _unitOfWork.PenaltiesRepository.JoinAndGetAllAsync(expression));
        }

        public async Task<IEnumerable<PenaltyDto>> GetAllNoRelatedAsync(Func<PenaltyModel, bool> func)
        {
            return _mapper.Map<IEnumerable<PenaltyModel>, PenaltyDto[]>(await _unitOfWork.PenaltiesRepository.GetAllAsync(func));
        }

        public async Task<PenaltyDto> GetByIdAsync(int id)
        {
            return _mapper.Map<PenaltyModel, PenaltyDto>(await _unitOfWork.PenaltiesRepository.GetAsync(id));
        }

        public async Task<PenaltyDto> GetAsync(Func<PenaltyModel, bool> func)
        {
            return _mapper.Map<PenaltyModel, PenaltyDto>(await _unitOfWork.PenaltiesRepository.GetAsync(func));
        }

        public async Task<PenaltyDto> CreateAsync(int creatorId, PenaltyDto dto)
        {
            dto.CreatorId = creatorId;
            dto.Date = DateTime.Now;
            PenaltyModel model = _mapper.Map<PenaltyDto, PenaltyModel>(dto);
            await _unitOfWork.PenaltiesRepository.InsertAsync(model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<PenaltyDto> UpdateAsync(int id, PenaltyDto dto)
        {
            PenaltyModel model = await _unitOfWork.PenaltiesRepository.JoinAndGetAsync(id);
            _unitOfWork.PenaltiesRepository.BeginUpdate(model);
            _mapper.Map(dto, model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<PenaltyDto> DeactivateAsync(int deactivatorId, int penaltyId)
        {
            PenaltyModel model = await _unitOfWork.PenaltiesRepository.JoinAndGetAsync(penaltyId);
            _unitOfWork.PenaltiesRepository.BeginUpdate(model);
            model.DeactivatorId = deactivatorId;
            model.Deactivated = true;
            model.DeactivationDate = DateTime.Now;
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PenaltyModel, PenaltyDto>(model);
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.PenaltiesRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ContainsAsync(int id)
        {
            return await _unitOfWork.PenaltiesRepository.ContainsAsync(id);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}