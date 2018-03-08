using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VRP.Core.Database.Models;

namespace vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class CharactersController : Controller
    {
        [HttpGet("{accountId}")]
        public JsonResult Get(int accountId)
        {
            CharacterModel character = new CharacterModel();
            character.Name = "John";
            character.Surname = "Doe";
            character.Money = 4000m;
            return Json(new
            {
                name = character.Name,
                surname = character.Surname,
                money = character.Money,
            });
        }
    }
}