using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
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
    public class ItemController : Controller
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;

        public ItemController(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        [HttpGet("characteritems")]
        public async Task<IActionResult> GetItemsByCurrentCharacterIdAsync()
        {
            int characterId = HttpContext.User.GetCharacterId();
            IEnumerable<ItemDto> items = await _itemService.GetAllAsync(item => item.CharacterId == characterId);
            return Json(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            ItemDto item = await _itemService.GetByIdAsync(id);

            if (item == null)
            {
                return NotFound(id);
            }

            return Json(item);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            IEnumerable<ItemDto> items = await _itemService.GetAllNoRelatedAsync();
            return Json(items);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", await _itemService.CreateAsync(HttpContext.User.GetAccountId(), itemDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(itemDto);
            }

            ItemDto item = await _itemService.UpdateAsync(id, itemDto);

            if (item == null)
            {
                return NotFound(id);
            }

            return Json(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _itemService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            await _itemService.DeleteAsync(id);

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _itemService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}