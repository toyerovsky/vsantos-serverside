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
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        
        [HttpGet("account/{id}")]
        public async Task<IActionResult> GetGroupsByAccountId(int id)
        {
            IEnumerable<GroupDto> groups = await _groupService.GetByAccountIdAsync(id);

            if (!groups.Any())
            {
                return NotFound(id);
            }

            return Json(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            GroupDto group = await _groupService.GetByIdAsync(id);

            if (group == null)
            {
                return NotFound(id);
            }

            return Json(group);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<GroupDto> groups = await _groupService.GetAllNoRelatedAsync();

            if (!groups.Any())
            {
                return NotFound();
            }

            return Json(groups);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GroupDto groupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Created("", await _groupService.CreateAsync(HttpContext.User.GetAccountId(), groupDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] GroupDto groupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(groupDto);
            }

            GroupDto group = await _groupService.UpdateAsync(id, groupDto);

            if (group == null)
            {
                return NotFound(id);
            }

            return Json(group);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _groupService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            await _groupService.DeleteAsync(id);

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _groupService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}