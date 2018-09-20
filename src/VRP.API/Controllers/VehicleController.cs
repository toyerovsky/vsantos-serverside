using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.API.Extensions;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;

namespace VRP.API.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("charactervehicles")]
        public async Task<IActionResult> GetVehiclesByCharacterIdAsync()
        {
            int characterId = HttpContext.User.GetCharacterId();
            IEnumerable<VehicleDto> vehicles = await _vehicleService.GetAllAsync(vehicle => vehicle.CharacterId == characterId);
            return Json(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            VehicleDto vehicle = await _vehicleService.GetByIdAsync(id);

            if (vehicle == null)
            {
                return NotFound(id);
            }

            return Json(vehicle);
        }

        [HttpGet("group/{id}")]
        public async Task<IActionResult> GetAllByGroupIdAsync(int id)
        {
            IEnumerable<VehicleDto> vehicles = await _vehicleService.GetAllAsync(v => v.GroupId == id);

            if (!vehicles.Any())
            {
                return NotFound(id);
            }

            return Json(vehicles);
        }

        [HttpGet("{numberPlate}")]
        public async Task<IActionResult> GetAsync(string numberPlate)
        {
            VehicleDto vehicle = await _vehicleService.GetAsync(veh => veh.NumberPlate == numberPlate);

            if (vehicle == null)
            {
                return NotFound(numberPlate);
            }

            return Json(vehicle);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            IEnumerable<VehicleDto> vehicles = await _vehicleService.GetAllAsync();
            return Json(vehicles);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] VehicleDto vehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", await _vehicleService.CreateAsync(HttpContext.User.GetAccountId(), vehicleDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] VehicleDto vehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(vehicleDto);
            }

            VehicleDto vehicle = await _vehicleService.UpdateAsync(id, vehicleDto);

            if (vehicle == null)
            {
                return NotFound(id);
            }

            return Json(vehicle);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _vehicleService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            await _vehicleService.DeleteAsync(id);

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _vehicleService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
