﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.BLL.Services;
using VRP.vAPI.Dto;
using VRP.vAPI.Extensions;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Penalty")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class PenaltyController : Controller
    {
        private readonly IPenaltyService _penaltyService;
        private readonly IMapper _mapper;

        public PenaltyController(IPenaltyService penaltyService, IMapper mapper)
        {
            _penaltyService = penaltyService;
            _mapper = mapper;
        }

        // GET: api/Penalty
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(await _penaltyService.GetAllAsync(null));
        }

        [HttpGet("account/{id}")]
        public async Task<IActionResult> GetByAccountId(int id)
        {
            IEnumerable<PenaltyDto> penalties =
                await _penaltyService.GetAllAsync(penalty => penalty.AccountId == id);

            if (!penalties.Any())
            {
                return NotFound(id);
            }

            return Json(penalties);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PenaltyDto penaltyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(penaltyDto);
            }

            PenaltyDto dto = await _penaltyService.CreateAsync(HttpContext.User.GetAccountId(), penaltyDto);
            return Created("", dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] PenaltyDto penaltyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(penaltyDto);
            }

            if (!await _penaltyService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            return Json(await _penaltyService.UpdateAsync(id, penaltyDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _penaltyService.ContainsAsync(id))
            {
                return NotFound(id);
            }

            await _penaltyService.DeleteAsync(id);

            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _penaltyService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
