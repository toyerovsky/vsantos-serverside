using System;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using VRP.Core.Repositories;
using VRP.vAPI.Model;
using VRP.vAPI.Services;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class CharactersController : Controller
    {
        private readonly CharactersRepository _charactersRepository = new CharactersRepository();
        private readonly IUsersWatcher _usersWatcher;

        public CharactersController(IUsersWatcher usersWatcher)
        {
            _usersWatcher = usersWatcher;
        }

        [HttpGet("{accountId}/account")]
        public JsonResult GetByAccountId(int accountId)
        {
            var characters = _charactersRepository.GetAll()
                .Where(character => character.Account.Id == accountId)
                .Select(character => new
                {
                    name = character.Name,
                    surname = character.Surname,
                    money = character.Money,
                });
            return Json(characters);
        }

        [HttpGet("{characterId}")]
        public JsonResult Get(int characterId)
        {
            return Json(_charactersRepository.Get(characterId));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _charactersRepository.Dispose();
        }
    }
}