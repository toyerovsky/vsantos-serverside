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
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        
        [HttpGet("account/{id}")]
        public async Task<IActionResult> GetGroupsByAccountIdAsync(int id)
        {
            IEnumerable<GroupDto> groups = await _groupService.GetByAccountIdAsync(id);
            return Json(groups);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            GroupDto group = await _groupService.GetByIdAsync(id);

            if (group == null)
            {
                return NotFound(id);
            }

            return Json(group);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            IEnumerable<GroupDto> groups = await _groupService.GetAllNoRelatedAsync();
            return Json(groups);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] GroupDto groupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Created("", await _groupService.CreateAsync(HttpContext.User.GetAccountId(), groupDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] GroupDto groupDto)
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
        public async Task<IActionResult> DeleteAsync(int id)
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