﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.UnitOfWork;

namespace VRP.BLL.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VehicleDto>> GetAllAsync(Expression<Func<VehicleModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<VehicleModel>, VehicleDto[]>(await _unitOfWork.VehiclesRepository.JoinAndGetAllAsync(expression));
        }

        public async Task<IEnumerable<VehicleDto>> GetAllNoRelatedAsync(Expression<Func<VehicleModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<VehicleModel>, VehicleDto[]>(await _unitOfWork.VehiclesRepository.GetAllAsync(expression));
        }

        public async Task<VehicleDto> GetByIdAsync(int id)
        {
            return _mapper.Map<VehicleModel, VehicleDto>(await _unitOfWork.VehiclesRepository.JoinAndGetAsync(id));
        }

        public async Task<VehicleDto> GetAsync(Expression<Func<VehicleModel, bool>> expression)
        {
            return _mapper.Map<VehicleModel, VehicleDto>(await _unitOfWork.VehiclesRepository.JoinAndGetAsync(expression));
        }

        public async Task<VehicleDto> CreateAsync(int creatorId, VehicleDto dto)
        {
            dto.Character = null;
            dto.Group = null;
            dto.CreatorId = creatorId;
            dto.CreationTime = DateTime.Now;
            dto.Health = 100;

            if (dto.Name == null)
                dto.Name = dto.VehicleHash;
            
            VehicleModel model = _mapper.Map<VehicleDto, VehicleModel>(dto);
            await _unitOfWork.VehiclesRepository.InsertAsync(model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task<VehicleDto> UpdateAsync(int id, VehicleDto dto)
        {
            dto.Character = null;
            dto.Group = null;
            VehicleModel model = await _unitOfWork.VehiclesRepository.GetAsync(id);
            _mapper.Map(dto, model);
            await _unitOfWork.SaveAsync();
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.VehiclesRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ContainsAsync(int id)
        {
            return await _unitOfWork.VehiclesRepository.ContainsAsync(id);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}