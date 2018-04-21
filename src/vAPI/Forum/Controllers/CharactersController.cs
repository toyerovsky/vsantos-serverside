using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models;
using VRP.Core.Interfaces;
using VRP.Core.Repositories;

namespace VRP.vAPI.Forum.Controllers
{
    [Produces("application/json")]
    [Route("api/forum/Characters")]
    public class CharactersController : Controller
    {
        private readonly IRepository<CharacterModel> _repository;

        public CharactersController(IRepository<CharacterModel> repository)
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