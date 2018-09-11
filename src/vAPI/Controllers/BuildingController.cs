using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.vAPI.Extensions;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class BuildingController : Controller
    {
        private readonly IBuildingService _buildingService;

        public BuildingController(IBuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            IEnumerable<BuildingDto> buildings = await _buildingService.GetAllNoRelatedAsync();

            if (!buildings.Any())
            {
                return NotFound();
            }

            return Json(buildings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            BuildingDto building = await _buildingService.GetByIdAsync(id);

            if (building == null)
            {
                return NotFound(id);
            }

            return Json(building);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] BuildingDto buildingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", await _buildingService.CreateAsync(HttpContext.User.GetAccountId(), buildingDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] BuildingDto buildingDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BuildingDto building = await _buildingService.UpdateAsync(id, buildingDto);

            if (building == null)
            {
                return NotFound(id);
            }

            return Json(building);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _buildingService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            await _buildingService.DeleteAsync(id);

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _buildingService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}