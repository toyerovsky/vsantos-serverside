using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.BLL.Dto;
using VRP.BLL.Services;
using VRP.vAPI.Extensions;

namespace VRP.vAPI.Controllers
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
        public async Task<IActionResult> GetItemsByCurrentCharacterId()
        {
            int characterId = HttpContext.User.GetCharacterId();
            IEnumerable<ItemDto> items = await _itemService.GetAllAsync(item => item.CharacterId == characterId);

            if (!items.Any())
            {
                return NotFound(characterId);
            }

            return Json(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!await _itemService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            return Json(await _itemService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<ItemDto> items = await _itemService.GetAllAsync();

            if (!items.Any())
            {
                return NotFound();
            }

            return Json(items);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", await _itemService.CreateAsync(HttpContext.User.GetAccountId(), itemDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(itemDto);
            }

            if (!await _itemService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            return Json(await _itemService.UpdateAsync(id, itemDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
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