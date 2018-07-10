using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Interfaces;
using VRP.vAPI.Extensions;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class VehicleController : Controller
    {
        private readonly IJoinableRepository<VehicleModel> _vehiclesRepository;

        public VehicleController(IJoinableRepository<VehicleModel> vehiclesRepository)
        {
            _vehiclesRepository = vehiclesRepository;
        }

        [HttpGet("charactervehicles")]
        public IActionResult GetVehiclesByCharacterId()
        {
            int characterId = HttpContext.User.GetCharacterId();
            IEnumerable<VehicleModel> vehicles = _vehiclesRepository.JoinAndGetAll(vehicle => vehicle.Character.Id == characterId);
            return Json(vehicles);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Json(_vehiclesRepository.Get(id));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vehiclesRepository?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
