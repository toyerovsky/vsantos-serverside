using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.DAL.Database.Models.Mdt;
using VRP.DAL.Interfaces;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class CriminalCaseController : Controller
    {
        private readonly IJoinableRepository<CriminalCaseModel> _criminalCaseRepository;

        public CriminalCaseController(IJoinableRepository<CriminalCaseModel> criminalCaseRepository)
        {
            _criminalCaseRepository = criminalCaseRepository;
        }

        // GET: api/criminalcase
        [HttpGet]
        public IActionResult Get()
        {
            return Json(_criminalCaseRepository.JoinAndGetAll());
        }

        // GET: api/criminalcase/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Json(_criminalCaseRepository.Get(id));
        }

        // POST: api/criminalcase
        [HttpPost]
        public IActionResult Post([FromBody]CriminalCaseModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _criminalCaseRepository.Insert(value);
            _criminalCaseRepository.Save();

            return Created("GET", value);
        }

        // PUT: api/criminalcase/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]CriminalCaseModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(value);
            }

            value.Id = id;
            _criminalCaseRepository.Update(value);
            _criminalCaseRepository.Save();

            return Ok(value);
        }

        // DELETE: api/criminalcase/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _criminalCaseRepository.Delete(id);
            _criminalCaseRepository.Save();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _criminalCaseRepository?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
