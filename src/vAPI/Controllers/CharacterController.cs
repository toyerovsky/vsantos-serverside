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
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.UnitOfWork;
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
        private readonly ICharacterService _characterService;
        // inject this to provide compability of old methods
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CharacterController(IConfiguration configuration, ICharacterService characterService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _configuration = configuration;
            _characterService = characterService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetByAccountId(int accountId)
        {
            IEnumerable<CharacterDto> characterDtos = await _characterService.GetAllAsync(character => character.AccountId == accountId);

            if (!characterDtos.Any())
            {
                return NotFound(accountId);
            }

            return Json(characterDtos);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<CharacterDto> characterDtos = await _characterService.GetAllAsync();

            if (!characterDtos.Any())
            {
                return NotFound();
            }

            return Json(characterDtos);
        }

        [HttpGet("account")]
        public IActionResult GetByCurrentUserCredentials()
        {
            var query = "SELECT Id, Name, Surname, Money, Model FROM vrpsrv.Characters WHERE AccountId = @accountId AND IsAlive = true";

            using (IDbConnection connection = new MySqlConnection(
                _configuration.GetConnectionString("gameConnectionString")))
            {
                int accountId = HttpContext.User.GetAccountId();

                using (var multiple = connection.QueryMultiple(query, new { accountId }))
                {
                    IEnumerable<CharacterModel> characters = multiple.Read<CharacterModel>();

                    var characterModels = characters as CharacterModel[] ?? characters.ToArray();

                    if (!characterModels.Any())
                    {
                        return NotFound(accountId);
                    }

                    IEnumerable<CharacterDto> characterDtos =
                        _mapper.Map<CharacterDto[]>(characterModels);

                    return Json(characterDtos);
                }
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (!await _characterService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            return Json(await _characterService.GetByIdAsync(id));
        }

        [HttpGet("selectedcharacter")]
        public IActionResult GetSelectedCharacter()
        {
            int characterId = HttpContext.User.GetCharacterId();
            CharacterModel character = _unitOfWork.CharactersRepository.Get(characterId);
            return Json(character);
        }

        [HttpPost("select")]
        public async Task<IActionResult> SelectCharacter([FromBody] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var characterTask = _characterService.GetByIdAsync(id);
            int accountId = HttpContext.User.GetAccountId();

            if ((await characterTask).AccountId != accountId)
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
        public async Task<IActionResult> Post([FromBody] CharacterDto characterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Created("", await _characterService.CreateAsync(characterDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] CharacterDto characterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _characterService.ContainsAsync(id))
            {
                return NotFound(id);
            }
            
            return Json(await _characterService.UpdateAsync(id, characterDto));
        }

        [HttpPut("image/{id}")]
        public async Task<IActionResult> UploadImage([FromRoute] int id, [FromBody] ImageDto imageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _characterService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            return Json(await _characterService.UpdateImageAsync(id, imageDto));
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