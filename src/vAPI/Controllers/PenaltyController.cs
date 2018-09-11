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
    public class PenaltyController : Controller
    {
        private readonly IPenaltyService _penaltyService;

        public PenaltyController(IPenaltyService penaltyService)
        {
            _penaltyService = penaltyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            IEnumerable<PenaltyDto> penalties = await _penaltyService.GetAllNoRelatedAsync();

            if (!penalties.Any())
            {
                return NotFound();
            }

            return Json(penalties);
        }

        [HttpGet("account/{id}")]
        public async Task<IActionResult> GetByAccountIdAsync(int id)
        {
            IEnumerable<PenaltyDto> penalties =
                await _penaltyService.GetAllAsync(penalty => penalty.AccountId == id);

            if (!penalties.Any())
            {
                return NotFound(id);
            }

            return Json(penalties);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            PenaltyDto penalty = await _penaltyService.GetByIdAsync(id);

            if (penalty == null)
            {
                return NotFound(id);
            }

            return Json(penalty);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] PenaltyDto penaltyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(penaltyDto);
            }

            return Created("", await _penaltyService.CreateAsync(HttpContext.User.GetAccountId(), penaltyDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] PenaltyDto penaltyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(penaltyDto);
            }

            PenaltyDto penalty = await _penaltyService.UpdateAsync(id, penaltyDto);

            if (penalty == null)
            {
                return NotFound(id);
            }

            return Json(penalty);
        }

        [HttpPut("deactivate/{id}")]
        [Authorize("Admin")]
        public async Task<IActionResult> DeactivateAsync([FromRoute] int id)
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
