using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Interfaces;
using VRP.vAPI.Dto;
using VRP.vAPI.Extensions;
using VRP.vAPI.UnitOfWork;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class VehicleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VehicleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("charactervehicles")]
        public IActionResult GetVehiclesByCharacterId()
        {
            int characterId = HttpContext.User.GetCharacterId();
            IEnumerable<VehicleModel> vehicles = _unitOfWork.VehiclesRepository.GetAll(vehicle => vehicle.CharacterId == characterId);

            if (!vehicles.Any())
            {
                return NotFound(characterId);
            }

            IEnumerable<VehicleDto> vehicleDtos = _mapper.Map<VehicleDto[]>(vehicles);
            return Json(vehicleDtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            VehicleModel vehicle = _unitOfWork.VehiclesRepository.Get(id);

            if (vehicle == null)
            {
                return NotFound(id);
            }

            VehicleDto vehicleDto = _mapper.Map<VehicleDto>(vehicle);
            return Json(vehicleDto);
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
