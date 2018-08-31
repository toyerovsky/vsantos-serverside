using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.vAPI.Extensions;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        [HttpGet("charactervehicles")]
        public async Task<IActionResult> GetVehiclesByCharacterId()
        {
            int characterId = HttpContext.User.GetCharacterId();
            IEnumerable<VehicleDto> vehicles = await _vehicleService.GetAllAsync(vehicle => vehicle.CharacterId == characterId);

            if (!vehicles.Any())
            {
                return NotFound(characterId);
            }

            return Json(vehicles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!await _vehicleService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            return Json(await _vehicleService.GetByIdAsync(id));
        }

        [HttpGet("{numberPlate}")]
        public async Task<IActionResult> Get(string numberPlate)
        {
            VehicleDto vehicle = await _vehicleService.GetAsync(veh => veh.NumberPlate == numberPlate);

            if (vehicle == null)
            {
                return NotFound(numberPlate);
            }

            return Json(vehicle);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<VehicleDto> vehicles = await _vehicleService.GetAllAsync();

            if (!vehicles.Any())
            {
                return NotFound();
            }

            return Json(vehicles);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VehicleDto vehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", await _vehicleService.CreateAsync(HttpContext.User.GetAccountId(), vehicleDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] VehicleDto vehicleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(vehicleDto);
            }

            if (!await _vehicleService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            return Json(await _vehicleService.UpdateAsync(id, vehicleDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
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
