using System.Collections.Generic;
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
    public class GroupRankController : Controller
    {
        private readonly IGroupRankService _groupRankService;

        public GroupRankController(IGroupRankService groupRankService)
        {
            _groupRankService = groupRankService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            GroupRankDto groupRank = await _groupRankService.GetByIdAsync(id);

            if (groupRank == null)
            {
                return NotFound(id);
            }

            return Json(groupRank);
        }

        [HttpGet("group/{id}")]
        public async Task<IActionResult> GetByGroupIdAsync(int id)
        {
            IEnumerable<GroupRankDto> groupRanks = await _groupRankService.GetByGroupIdAsync(id);
            return Json(groupRanks);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            IEnumerable<GroupRankDto> groupRanks = await _groupRankService.GetAllNoRelatedAsync();
            return Json(groupRanks);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] GroupRankDto groupRankDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", await _groupRankService.CreateAsync(HttpContext.User.GetAccountId(), groupRankDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] GroupRankDto groupRankDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(groupRankDto);
            }

            GroupRankDto groupRank = await _groupRankService.UpdateAsync(id, groupRankDto);

            if (groupRank == null)
            {
                return NotFound(id);
            }

            return Json(groupRank);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _groupRankService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            await _groupRankService.DeleteAsync(id);

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _groupRankService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
