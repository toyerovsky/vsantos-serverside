/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database.Models;
using VRP.Core.Database.Models.Character;
using VRP.Core.Interfaces;

namespace VRP.vAPI.Forum.Controllers
{
    [Produces("application/json")]
    [Route("api/forum/Characters")]
    public class CharacterController : Controller
    {
        private readonly IRepository<CharacterModel> _repository;

        public CharacterController(IRepository<CharacterModel> repository)
        {
            _repository = repository;
        }

        // GET: api/Characters
        [HttpGet]
        public IEnumerable<CharacterModel> Get()
        {
            return _repository.GetAll();
        }

        // GET: api/Characters/5
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var characterModel = _repository.Get(id);

            if (characterModel == null)
            {
                return NotFound();
            }

            return Ok(characterModel);
        }

        // PUT: api/Characters/5
        [HttpPut("{id}")]
        public ActionResult Put([FromRoute] int id, [FromBody] CharacterModel characterModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != characterModel.Id)
            {
                return BadRequest();
            }

            _repository.Update(characterModel);

            try
            {
                _repository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repository.Contains(characterModel))
                {
                    return NotFound();
                }
            }

            return NoContent();
        }

        // POST: api/Characters
        [HttpPost]
        public IActionResult Post([FromBody] CharacterModel characterModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Insert(characterModel);
            _repository.Save();

            return CreatedAtAction("Get", new { id = characterModel.Id }, characterModel);
        }

        // DELETE: api/Characters/5
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var characterModel = _repository.Get(id);
            if (characterModel == null)
            {
                return NotFound();
            }

            _repository.Delete(characterModel.Id);
            _repository.Save();

            return Ok(characterModel);
        }
    }
}