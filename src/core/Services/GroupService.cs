﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Group;
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
            return _mapper.Map<IEnumerable<GroupModel>, IEnumerable<GroupDto>>(await _unitOfWork.GroupsRepository.JoinAndGetAllAsync(expression));
        }

        public async Task<IEnumerable<GroupDto>> GetAllNoRelatedAsync(Func<GroupModel, bool> func)
        {
            return _mapper.Map<IEnumerable<GroupModel>, IEnumerable<GroupDto>>(await _unitOfWork.GroupsRepository.GetAllAsync(func));
        }

        public async Task<GroupDto> GetByIdAsync(int id)
        {
            return _mapper.Map<GroupModel, GroupDto>(await _unitOfWork.GroupsRepository.GetAsync(id));
        }

        public async Task<GroupDto> GetAsync(Func<GroupModel, bool> func)
        {
            return _mapper.Map<GroupModel, GroupDto>(await _unitOfWork.GroupsRepository.GetAsync(func));
        }

        public async Task<GroupDto> CreateAsync(int creatorId, GroupDto dto)
        {
            dto.Money = 0;
            dto.CreatorId = creatorId;
            dto.CreationTime = DateTime.Now;
            GroupModel model = _mapper.Map<GroupDto, GroupModel>(dto);
            await _unitOfWork.GroupsRepository.InsertAsync(model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<GroupDto> UpdateAsync(int id, GroupDto dto)
        {
            GroupModel model = await _unitOfWork.GroupsRepository.JoinAndGetAsync(id);
            _unitOfWork.GroupsRepository.BeginUpdate(model);
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
            GroupModel groupModel = _unitOfWork.GroupsRepository.Get(characterId);
            _unitOfWork.GroupsRepository.BeginUpdate(groupModel);
            groupModel.ImageUploadDate = DateTime.Now;
            groupModel.ImageUrl = await imageTask;
            await _unitOfWork.SaveAsync();
            return Mapper.Map<GroupModel, GroupDto>(groupModel);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}