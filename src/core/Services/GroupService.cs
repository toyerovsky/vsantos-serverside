using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Enums;
using VRP.DAL.UnitOfWork;

namespace VRP.BLL.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;

        public GroupService(IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupDto>> GetAllAsync(Expression<Func<GroupModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<GroupModel>, GroupDto[]>(await _unitOfWork.GroupsRepository.JoinAndGetAllAsync(expression));
        }

        public async Task<IEnumerable<GroupDto>> GetAllNoRelatedAsync(Expression<Func<GroupModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<GroupModel>, GroupDto[]>(await _unitOfWork.GroupsRepository.GetAllAsync(expression));
        }

        public async Task<GroupDto> GetByIdAsync(int id)
        {
            return _mapper.Map<GroupModel, GroupDto>(await _unitOfWork.GroupsRepository.JoinAndGetAsync(id));
        }

        public async Task<GroupDto> GetAsync(Expression<Func<GroupModel, bool>> expression)
        {
            return _mapper.Map<GroupModel, GroupDto>(await _unitOfWork.GroupsRepository.JoinAndGetAsync(expression));
        }

        public async Task<GroupDto> CreateAsync(int creatorId, GroupDto dto)
        {
            dto.Money = 0;
            dto.CreatorId = creatorId;
            dto.CreationTime = DateTime.Now;
            dto.BossCharacter = null;

            GroupModel group = _mapper.Map<GroupDto, GroupModel>(dto);
            
            GroupRankModel groupRank = new GroupRankModel
            {
                Name = "Lider",
                Rights = GroupRights.AllBasic,
            };

            group.DefaultRank = groupRank;

            WorkerModel worker = new WorkerModel
            {
                CharacterId = group.BossCharacterId,
                Group = group,
            };

            groupRank.Workers.Add(worker);
            group.GroupRanks.Add(groupRank);

            await _unitOfWork.GroupsRepository.InsertAsync(group);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<GroupDto> UpdateAsync(int id, GroupDto dto)
        {
            dto.BossCharacter = null;
            GroupModel model = await _unitOfWork.GroupsRepository.GetAsync(id);
            _mapper.Map(dto, model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.GroupsRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ContainsAsync(int id)
        {
            return await _unitOfWork.GroupsRepository.ContainsAsync(id);
        }

        public async Task<GroupDto> UpdateImageAsync(int characterId, ImageDto imageDto)
        {
            var imageTask = _imageService.UploadImageAsync(imageDto);
            GroupModel groupModel = await _unitOfWork.GroupsRepository.GetAsync(characterId);
            groupModel.ImageUploadDate = DateTime.Now;
            groupModel.ImageUrl = await imageTask;
            await _unitOfWork.SaveAsync();
            return _mapper.Map<GroupModel, GroupDto>(groupModel);
        }

        public async Task<IEnumerable<GroupDto>> GetByAccountIdAsync(int id)
        {
            return _mapper.Map<IEnumerable<GroupModel>, GroupDto[]>(
                await _unitOfWork.GroupsRepository.JoinAndGetAllAsync(group => group.Workers.Any(worker => worker.Character.AccountId == id)));
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}