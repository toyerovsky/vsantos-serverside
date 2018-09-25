using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.UnitOfWork;

namespace VRP.BLL.Services
{
    public class CrimeBotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CrimeBotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CrimeBotDto>> GetAllAsync(Expression<Func<CrimeBotModel, bool>> expression = null)
        {
            return _mapper.Map<IEnumerable<CrimeBotModel>, CrimeBotDto[]>(await _unitOfWork.CrimeBotsRepository.JoinAndGetAllAsync(expression));
        }
        public async Task<IEnumerable<CrimeBotDto>> GetAllNoRelatedAsync(Expression<Func<CrimeBotModel, bool>> expression = null)
        {
            return _mapper.Map<IEnumerable<CrimeBotModel>, CrimeBotDto[]>(await _unitOfWork.CrimeBotsRepository.GetAllAsync(expression));
        }
        public async Task<CrimeBotDto> GetByIdAsync(int id)
        {
            return _mapper.Map<CrimeBotModel, CrimeBotDto>(await _unitOfWork.CrimeBotsRepository.JoinAndGetAsync(id));
        }
        public async Task<CrimeBotDto> GetAsync(Expression<Func<CrimeBotModel, bool>> expression = null)
        {
            return _mapper.Map<CrimeBotModel, CrimeBotDto>(await _unitOfWork.CrimeBotsRepository.JoinAndGetAsync(expression));
        }
        public async Task<IEnumerable<CrimeBotDto>> GetByCrimeBotIdAsync(int id)
        {
            return _mapper.Map<IEnumerable<CrimeBotModel>, CrimeBotDto[]>(
                await _unitOfWork.CrimeBotsRepository.JoinAndGetAllAsync(crimeBot => crimeBot.Id == id));
        }

        public async Task<CrimeBotDto> CreateAsync(CrimeBotDto dto)
        {
            CrimeBotModel model = _mapper.Map<CrimeBotDto, CrimeBotModel>(dto);
            await _unitOfWork.CrimeBotsRepository.InsertAsync(model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<CrimeBotDto> UpdateAsync(int id, CrimeBotDto dto)
        {
            dto.VehicleModel = null;
            dto.GroupModelId = null;
            dto.BotModelId = null;
            CrimeBotModel model = await _unitOfWork.CrimeBotsRepository.JoinAndGetAsync(id);
            _mapper.Map(dto, model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.CrimeBotsRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ContainsAsync(int id)
        {
            return await _unitOfWork.CrimeBotsRepository.ContainsAsync(id);
        }
        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}
