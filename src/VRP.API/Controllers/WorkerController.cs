using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;

namespace VRP.API.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class WorkerController : Controller
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpGet("group/{id}")]
        public async Task<IActionResult> GetWorkersByGroupIdAsync(int id)
        {
            IEnumerable<WorkerDto> workers = await _workerService.GetByGroupIdAsync(id);
            return Json(workers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            WorkerDto worker = await _workerService.GetByIdAsync(id);

            if (worker == null)
            {
                return NotFound(id);
            }

            return Json(worker);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            IEnumerable<WorkerDto> workers = await _workerService.GetAllNoRelatedAsync();
            return Json(workers);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] WorkerDto workerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Created("", await _workerService.CreateAsync(workerDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] WorkerDto workerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(workerDto);
            }

            WorkerDto worker = await _workerService.UpdateAsync(id, workerDto);

            if (worker == null)
            {
                return NotFound(id);
            }

            return Json(worker);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _workerService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            await _workerService.DeleteAsync(id);

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _workerService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}