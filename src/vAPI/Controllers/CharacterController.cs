/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VRP.Core.Database;
using VRP.Core.Database.Models.Character;
using VRP.Core.Repositories;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class CharacterController : Controller
    {
        private readonly IConfiguration _configuration;

        public CharacterController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("account/{accountId}")]
        public IActionResult GetByAccountId(int accountId)
        {
            var query = "SELECT Name, Surname, Money FROM vrpsrv.Characters WHERE AccountId = @accountId AND IsAlive = true";

            using (IDbConnection connection = new MySqlConnection(
                _configuration.GetConnectionString("gameConnectionString")))
            {
                using (var multiple = connection.QueryMultiple(query, new { accountId }))
                {
                    var characters = multiple.Read().Select(character => new
                    {
                        name = character.Name,
                        surname = character.Surname,
                        money = character.Money
                    });
                    return Json(characters);
                }
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CharacterModel characterModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (CharactersRepository repository = new CharactersRepository())
            {
                repository.Insert(characterModel);
                repository.Save();
            }

            return Created("GET", characterModel);
        }
    }
}