using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.UnitOfWork;
using VRP.vAPI.Extensions;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
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
        public async Task<IActionResult> Get(int id)
        {
            if (!await _groupRankService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            return Json(await _groupRankService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<GroupRankDto> groupRanks = await _groupRankService.GetAllAsync();

            if (!groupRanks.Any())
            {
                return NotFound();
            }

            return Json(groupRanks);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GroupRankDto groupRankDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", await _groupRankService.CreateAsync(HttpContext.User.GetAccountId(), groupRankDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] GroupRankDto groupRankDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(groupRankDto);
            }

            if (!await _groupRankService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            return Json(await _groupRankService.UpdateAsync(id, groupRankDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
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
