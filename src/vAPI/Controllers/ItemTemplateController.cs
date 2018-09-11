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
    public class ItemTemplateController : Controller
    {
        private readonly IItemTemplateService _itemTemplateService;

        public ItemTemplateController(IItemTemplateService itemTemplateService)
        {
            _itemTemplateService = itemTemplateService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            ItemTemplateDto itemTemplate = await _itemTemplateService.GetByIdAsync(id);

            if (itemTemplate == null)
            {
                return NotFound(id);
            }

            return Json(itemTemplate);
        }


        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            IEnumerable<ItemTemplateDto> itemTemplates = await _itemTemplateService.GetAllNoRelatedAsync();

            if (!itemTemplates.Any())
            {
                return NotFound();
            }

            return Json(itemTemplates);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ItemTemplateDto itemTemplateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", await _itemTemplateService.CreateAsync(HttpContext.User.GetAccountId(), itemTemplateDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] ItemTemplateDto itemTemplateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(itemTemplateDto);
            }

            ItemTemplateDto itemTemplate = await _itemTemplateService.UpdateAsync(id, itemTemplateDto);

            if (itemTemplate == null)
            {
                return NotFound(id);
            }

            return Json(itemTemplate);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _itemTemplateService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            await _itemTemplateService.DeleteAsync(id);

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _itemTemplateService?.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}