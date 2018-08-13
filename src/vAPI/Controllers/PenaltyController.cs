using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.DAL.Database.Models.Account;
using VRP.vAPI.Dto;
using VRP.vAPI.UnitOfWork;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Penalty")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class PenaltyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PenaltyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/Penalty
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<PenaltyModel> penalties = _unitOfWork.PenaltiesRepository.GetAll();

            if (!penalties.Any())
            {
                return NotFound();
            }

            IEnumerable<PenaltyDto> penaltyDtos = _mapper.Map<IEnumerable<PenaltyDto>>(penalties);
            return Json(penaltyDtos);
        }

        [HttpGet("account/{id}")]
        public IActionResult GetByAccountId(int id)
        {
            IEnumerable<PenaltyModel> penalties =
                _unitOfWork.PenaltiesRepository.JoinAndGetAll(penalty => penalty.AccountId == id);

            if (!penalties.Any())
            {
                return NotFound(id);
            }

            IEnumerable<PenaltyDto> penaltyDtos = _mapper.Map<PenaltyDto[]>(penalties);
            return Json(penaltyDtos);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PenaltyDto penaltyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(penaltyDto);
            }

            PenaltyModel penalty = _mapper.Map<PenaltyModel>(penaltyDto);

            _unitOfWork.PenaltiesRepository.Insert(penalty);
            _unitOfWork.PenaltiesRepository.Save();

            return Created("GET", penalty);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] PenaltyDto penaltyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(penaltyDto);
            }

            PenaltyModel penalty = _unitOfWork.PenaltiesRepository.JoinAndGet(id);

            if (penalty == null)
            {
                return NotFound(id);
            }

            _unitOfWork.PenaltiesRepository.BeginUpdate(penalty);
            _mapper.Map(penaltyDto, penalty);
            _unitOfWork.PenaltiesRepository.Save();

            return Json(_mapper.Map<PenaltyDto>(penalty));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_unitOfWork.PenaltiesRepository.Contains(id))
            {
                return NotFound(id);
            }

            _unitOfWork.PenaltiesRepository.Delete(id);
            _unitOfWork.PenaltiesRepository.Save();

            return NoContent();
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
