using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.UnitOfWork;

namespace VRP.BLL.Services
{
    public class GroupRankService : IGroupRankService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GroupRankService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupRankDto>> GetAllAsync(Expression<Func<GroupRankModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<GroupRankModel>, GroupRankDto[]>((await _unitOfWork.GroupRanksRepository.JoinAndGetAllAsync(expression)).ToArray());
        }

        public async Task<IEnumerable<GroupRankDto>> GetAllNoRelatedAsync(Expression<Func<GroupRankModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<GroupRankModel>, GroupRankDto[]>((await _unitOfWork.GroupRanksRepository.GetAllAsync(expression)).ToArray());
        }

        public async Task<GroupRankDto> GetByIdAsync(int id)
        {
            return _mapper.Map<GroupRankModel, GroupRankDto>(await _unitOfWork.GroupRanksRepository.GetAsync(id));
        }

        public async Task<GroupRankDto> GetAsync(Expression<Func<GroupRankModel, bool>> expression)
        {
            return _mapper.Map<GroupRankModel, GroupRankDto>(await _unitOfWork.GroupRanksRepository.GetAsync(expression));
        }

        public async Task<GroupRankDto> CreateAsync(int creatorId, GroupRankDto dto)
        {
            GroupRankModel model = _mapper.Map<GroupRankDto, GroupRankModel>(dto);
            await _unitOfWork.GroupRanksRepository.InsertAsync(model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<GroupRankDto> UpdateAsync(int id, GroupRankDto dto)
        {
            GroupRankModel model = await _unitOfWork.GroupRanksRepository.JoinAndGetAsync(id);
            _unitOfWork.GroupRanksRepository.BeginUpdate(model);
            _mapper.Map(dto, model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.GroupRanksRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ContainsAsync(int id)
        {
            return await _unitOfWork.GroupRanksRepository.ContainsAsync(id);
        }

        public async Task<IEnumerable<GroupRankDto>> GetByGroupIdAsync(int id)
        {
            return _mapper.Map<IEnumerable<GroupRankModel>, GroupRankDto[]>((
                await _unitOfWork.GroupRanksRepository.JoinAndGetAllAsync(groupRank => groupRank.GroupId == id)).ToArray());
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}