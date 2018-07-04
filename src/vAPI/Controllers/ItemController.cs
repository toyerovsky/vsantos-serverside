using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories;
using VRP.vAPI.Extensions;

namespace vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class ItemController : Controller
    {
        private readonly IJoinableRepository<ItemModel> _itemsRepository;

        public ItemController(IJoinableRepository<ItemModel> itemsRepository)
        {
            _itemsRepository = itemsRepository;
        }

        [HttpGet("characteritems")]
        public IActionResult GetItemsByCharacterId()
        {
            int characterId = HttpContext.User.GetCharacterId();
            IEnumerable<ItemModel> items = _itemsRepository.JoinAndGetAll(item => item.Character.Id == characterId);
            return Json(items);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Json(_itemsRepository.Get(id));
        }
    }
}