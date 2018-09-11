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
    public class WorkerService : IWorkerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WorkerDto>> GetAllAsync(Expression<Func<WorkerModel, bool>> expression = null)
        {
            return _mapper.Map<IEnumerable<WorkerModel>, WorkerDto[]>(await _unitOfWork.WorkersRepository.JoinAndGetAllAsync(expression));
        }

        public async Task<IEnumerable<WorkerDto>> GetAllNoRelatedAsync(Expression<Func<WorkerModel, bool>> expression = null)
        {
            return _mapper.Map<IEnumerable<WorkerModel>, WorkerDto[]>(await _unitOfWork.WorkersRepository.GetAllAsync(expression));
        }

        public async Task<WorkerDto> GetByIdAsync(int id)
        {
            return _mapper.Map<WorkerModel, WorkerDto>(await _unitOfWork.WorkersRepository.JoinAndGetAsync(id));
        }

        public async Task<WorkerDto> GetAsync(Expression<Func<WorkerModel, bool>> expression = null)
        {
            return _mapper.Map<WorkerModel, WorkerDto>(await _unitOfWork.WorkersRepository.JoinAndGetAsync(expression));
        }

        public async Task<IEnumerable<WorkerDto>> GetByGroupIdAsync(int id)
        {
            return _mapper.Map<IEnumerable<WorkerModel>, WorkerDto[]>(
                await _unitOfWork.WorkersRepository.JoinAndGetAllAsync(worker => worker.GroupId == id));
        }

        public async Task<WorkerDto> CreateAsync(WorkerDto dto)
        {
            WorkerModel model = _mapper.Map<WorkerDto, WorkerModel>(dto);
            await _unitOfWork.WorkersRepository.InsertAsync(model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<WorkerDto> UpdateAsync(int id, WorkerDto dto)
        {
            dto.Character = null;
            dto.Group = null;
            dto.GroupRank = null;
            WorkerModel model = await _unitOfWork.WorkersRepository.JoinAndGetAsync(id);
            _mapper.Map(dto, model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.WorkersRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ContainsAsync(int id)
        {
            return await _unitOfWork.WorkersRepository.ContainsAsync(id);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}