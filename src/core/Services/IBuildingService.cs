﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Building;

namespace VRP.BLL.Services
{
    public interface IBuildingService : IDisposable
    {
        Task<IEnumerable<BuildingDto>> GetAllAsync(Expression<Func<BuildingModel, bool>> expression = null);
        Task<IEnumerable<BuildingDto>> GetAllNoRelatedAsync(Func<BuildingModel, bool> func = null);
        Task<BuildingDto> GetByIdAsync(int id);
        Task<BuildingDto> GetAsync(Func<BuildingModel, bool> func = null);
        Task<BuildingDto> UpdateAsync(int id, BuildingDto dto);
        Task<BuildingDto> CreateAsync(int creatorId, BuildingDto dto);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
    }
}