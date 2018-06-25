/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories;
using VRP.vAPI.Extensions;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class CharacterController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<CharacterModel> _charactersRepository;

        public CharacterController(IConfiguration configuration, IRepository<CharacterModel> charactersRepository)
        {
            _configuration = configuration;
            _charactersRepository = charactersRepository;
        }

        [HttpGet("account")]
        public IActionResult GetByAccountId()
        {
            var query = "SELECT Id, Name, Surname, Money, Model FROM vrpsrv.Characters WHERE AccountId = @accountId AND IsAlive = true";

            using (IDbConnection connection = new MySqlConnection(
                _configuration.GetConnectionString("gameConnectionString")))
            {
                using (var multiple = connection.QueryMultiple(query, new { characterId = HttpContext.User.GetAccountId() }))
                {
                    var characters = multiple.Read<CharacterModel>().Select(character => new
                    {
                        name = character.Name,
                        surname = character.Surname,
                        money = character.Money,
                        model = character.Model
                    });
                    return Json(characters);
                }
            }
        }

        [HttpPost("select")]
        public async Task<IActionResult> SelectCharacter([FromBody] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Task<CharacterModel> charactersTask = _charactersRepository.GetAsync(id);

            int accountId = HttpContext.User.GetAccountId();

            if ((await charactersTask).AccountId != accountId)
            {
                return Forbid();
            }

            IEnumerable<Claim> claims = new List<Claim>()
                {
                    new Claim("CharacterId", id.ToString())
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            HttpContext.User.AddIdentity(claimsIdentity);

            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] CharacterModel characterModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            characterModel.AccountId = HttpContext.User.GetAccountId();
            _charactersRepository.Insert(characterModel);
            _charactersRepository.Save();

            return Created("GET", characterModel);
        }
    }
}