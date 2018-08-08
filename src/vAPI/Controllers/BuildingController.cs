using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Building;
using VRP.vAPI.Dto;
using VRP.vAPI.Model;
using VRP.vAPI.UnitOfWork;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class BuildingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BuildingController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<BuildingModel> buildings = _unitOfWork.BuildingsRepository.GetAll();

            if (!buildings.Any())
            {
                return NotFound();
            }

            IEnumerable<BuildingDto> buildingDtos = _mapper.Map<BuildingDto[]>(buildings);
            return Json(buildingDtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            BuildingModel building = _unitOfWork.BuildingsRepository.Get(id);

            if (building == null)
            {
                return NotFound(id);
            }

            BuildingDto buildingDto = _mapper.Map<BuildingDto>(building);
            return Json(buildingDto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}