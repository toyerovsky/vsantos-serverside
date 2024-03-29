﻿ /* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.API.Extensions;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.UnitOfWork;

namespace VRP.API.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [EnableCors("dev")]
    //[Authorize("Authenticated")]
    public class CharacterController : Controller
    { 
        private readonly ICharacterService _characterService;
        // inject this to provide compability of old methods
        private readonly IUnitOfWork _unitOfWork;

        public CharacterController(ICharacterService characterService, IUnitOfWork unitOfWork)
        {
            _characterService = characterService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetByAccountIdAsync(int accountId)
        {
            IEnumerable<CharacterDto> characterDtos = await _characterService.GetAllAsync(character => character.AccountId == accountId);
            return Json(characterDtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            IEnumerable<CharacterDto> characterDtos = await _characterService.GetAllNoRelatedAsync();
            return Json(characterDtos);
        }

        [HttpGet("account")]
        public async Task<IActionResult> GetByCurrentUserCredentialsAsync()
        {
            IEnumerable<CharacterDto> characterDtos = await _characterService.GetAllNoRelatedAsync(
                character => character.AccountId == HttpContext.User.GetAccountId());

            if (!characterDtos.Any())
            {
                return NotFound();
            }

            return Json(characterDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            CharacterDto character = await _characterService.GetByIdAsync(id);

            if (character == null)
            {
                return NotFound(id);
            }

            return Json(character);
        }

        [HttpGet("selectedcharacter")]
        public IActionResult GetSelectedCharacter()
        {
            int characterId = HttpContext.User.GetCharacterId();
            CharacterModel character = _unitOfWork.CharactersRepository.Get(characterId);
            return Json(character);
        }

        [HttpPost("select")]
        public async Task<IActionResult> SelectCharacterAsync([FromBody] int id)
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
        public async Task<IActionResult> PostAsync([FromBody] CharacterDto characterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", await _characterService.CreateAsync(characterDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] CharacterDto characterDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CharacterDto character = await _characterService.UpdateAsync(id, characterDto);

            if (character == null)
            {
                return NotFound(id);
            }

            return Json(character);
        }

        [HttpPut("image/{id}")]
        public async Task<IActionResult> UploadImageAsync([FromRoute] int id, [FromBody] ImageDto imageDto)
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