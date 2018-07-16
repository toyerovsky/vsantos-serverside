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
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Interfaces;
using VRP.vAPI.Dto;
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
        private readonly IMapper _mapper;

        public CharacterController(IConfiguration configuration, IJoinableRepository<CharacterModel> charactersRepository, IMapper mapper)
        {
            _configuration = configuration;
            _charactersRepository = charactersRepository;
            _mapper = mapper;
        }

        [HttpGet("account/{accountId}")]
        public IActionResult GetByAccountId(int accountId)
        {
            IEnumerable<CharacterModel> characters = _charactersRepository.GetAll(character => character.AccountId == accountId);

            var characterModels = characters as CharacterModel[] ?? characters.ToArray();

            if (!characterModels.Any())
            {
                return NotFound(accountId);
            }

            IEnumerable<CharacterDto> characterDtos =
                _mapper.Map<IEnumerable<CharacterModel>, IEnumerable<CharacterDto>>(characterModels);

            return Json(characterDtos);
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<CharacterModel> characters = _charactersRepository.GetAll();

            var characterModels = characters as CharacterModel[] ?? characters.ToArray();

            if (!characterModels.Any())
            {
                return NotFound();
            }

            IEnumerable<CharacterDto> characterDtos =
                _mapper.Map<IEnumerable<CharacterModel>, IEnumerable<CharacterDto>>(characterModels);

            return Json(characterDtos);
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
        public IActionResult Post([FromBody] CharacterDto characterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CharacterModel character = _mapper.Map<CharacterModel>(characterDto);
            character.IsAlive = true;

            _charactersRepository.Insert(character);
            _charactersRepository.Save();

            return Created("GET", character);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] CharacterDto characterDto)
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
            _mapper.Map(characterDto, character);
            _charactersRepository.Save();

            return Json(character);
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