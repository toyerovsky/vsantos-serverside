using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Interfaces;
using VRP.vAPI.Dto;
using VRP.vAPI.Extensions;
using VRP.vAPI.UnitOfWork;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class ItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItemController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("characteritems")]
        public IActionResult GetItemsByCurrentCharacterId()
        {
            int characterId = HttpContext.User.GetCharacterId();
            IEnumerable<ItemModel> items = _unitOfWork.ItemsRepository.GetAll(item => item.CharacterId == characterId);

            if (!items.Any())
            {
                return NotFound(characterId);
            }

            IEnumerable<ItemDto> itemDtos = _mapper.Map<ItemDto[]>(items);
            return Json(itemDtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            ItemModel item = _unitOfWork.ItemsRepository.Get(id);

            if (item == null)
            {
                return NotFound(id);
            }

            ItemDto itemDto = _mapper.Map<ItemDto>(item);
            return Json(itemDto);
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<ItemModel> items = _unitOfWork.ItemsRepository.GetAll();

            var itemModels = items as ItemModel[] ?? items.ToArray();

            if (!itemModels.Any())
            {
                return NotFound();
            }

            IEnumerable<ItemDto> itemDtos =
                _mapper.Map<ItemDto[]>(itemModels);

            return Json(itemDtos);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ItemModel item = _mapper.Map<ItemModel>(itemDto);

            _unitOfWork.ItemsRepository.Insert(item);
            _unitOfWork.ItemsRepository.Save();

            return Created("GET", item);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] ItemDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(itemDto);
            }

            ItemModel item = _unitOfWork.ItemsRepository.JoinAndGet(id);

            if (item == null)
            {
                return NotFound(id);
            }

            _unitOfWork.ItemsRepository.BeginUpdate(item);
            _mapper.Map(itemDto, item);
            _unitOfWork.ItemsRepository.Save();

            return Json(_mapper.Map<ItemDto>(item));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_unitOfWork.GroupsRepository.Contains(id))
            {
                return NotFound(id);
            }

            _unitOfWork.ItemsRepository.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}