using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.BLL.Dto;
using VRP.BLL.Services;
using VRP.vAPI.Extensions;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Penalty")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class PenaltyController : Controller
    {
        private readonly IPenaltyService _penaltyService;

        public PenaltyController(IPenaltyService penaltyService)
        {
            _penaltyService = penaltyService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(await _penaltyService.GetAllAsync());
        }

        [HttpGet("account/{id}")]
        public async Task<IActionResult> GetByAccountId(int id)
        {
            IEnumerable<PenaltyDto> penalties =
                await _penaltyService.GetAllAsync(penalty => penalty.AccountId == id);

            if (!penalties.Any())
            {
                return NotFound(id);
            }

            return Json(penalties);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PenaltyDto penaltyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(penaltyDto);
            }

            PenaltyDto dto = await _penaltyService.CreateAsync(HttpContext.User.GetAccountId(), penaltyDto);
            return Created("", dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PenaltyDto penaltyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(penaltyDto);
            }

            if (!await _penaltyService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            return Json(await _penaltyService.UpdateAsync(id, penaltyDto));
        }

        [HttpPut("deactivate/{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> Put([FromRoute] int id)
        {
            if (!await _penaltyService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            return Json(await _penaltyService.DeactivateAsync(User.GetAccountId(), id));
        }

        [HttpDelete("{id}")]
        [Authorize("Management")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _penaltyService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            await _penaltyService.DeleteAsync(id);

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _penaltyService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
