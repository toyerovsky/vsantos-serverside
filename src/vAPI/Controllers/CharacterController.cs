﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Interfaces;
using VRP.vAPI.Extensions;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class CharacterController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IJoinableRepository<CharacterModel> _charactersRepository;
        private readonly IJoinableRepository<AccountModel> _accountsRepository;

        public CharacterController(IConfiguration configuration, IJoinableRepository<CharacterModel> charactersRepository,
            IJoinableRepository<AccountModel> accountsRepository)
        {
            _configuration = configuration;
            _charactersRepository = charactersRepository;
            _accountsRepository = accountsRepository;
        }

        [HttpGet("account/{accountId}")]
        public IActionResult GetByAccountId(int accountId)
        {
            if (!_accountsRepository.Contains(accountId))
            {
                return NotFound(accountId);
            }

            IEnumerable<CharacterModel> characters = _accountsRepository.JoinAndGet(accountId).Characters;
            return Json(characters);
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<CharacterModel> characters = _charactersRepository.JoinAndGetAll();
            return Json(characters);
        }

        [HttpGet("account")]
        public IActionResult GetByCurrentUserCredentials()
        {
            var query = "SELECT Id, Name, Surname, Money, Model FROM vrpsrv.Characters WHERE AccountId = @accountId AND IsAlive = true";

            using (IDbConnection connection = new MySqlConnection(
                _configuration.GetConnectionString("gameConnectionString")))
            {
                using (var multiple = connection.QueryMultiple(query, new { accountId = HttpContext.User.GetAccountId() }))
                {
                    var characters = multiple.Read<CharacterModel>().Select(character => new
                    {
                        id = character.Id,
                        name = character.Name,
                        surname = character.Surname,
                        money = character.Money,
                        model = character.Model
                    });
                    return Json(characters);
                }
            }
        }

        [HttpGet("selectedcharacter")]
        public IActionResult GetSelectedCharacter()
        {
            int characterId = HttpContext.User.GetCharacterId();
            CharacterModel character = _charactersRepository.Get(characterId);
            return Json(character);
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

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("CharacterId", id.ToString()));
            HttpContext.User.AddIdentity(claimsIdentity);

            await HttpContext.SignInAsync(HttpContext.User);
            return Ok();
        }

        [HttpPost]
        public IActionResult Post([FromBody] CharacterModel characterModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _charactersRepository.Insert(characterModel);
            _charactersRepository.Save();

            return Created("GET", characterModel);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] string updatedCharacterJson)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CharacterModel character = _charactersRepository.Get(id);

            if (character == null)
            {
                return NotFound(id);
            }

            _charactersRepository.BeginUpdate(character);
            character.UpdateFieldsFromJson(updatedCharacterJson);
            _charactersRepository.Save();

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _charactersRepository?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}