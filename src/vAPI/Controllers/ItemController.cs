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
    }
}