using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.Core.Extensions;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Interfaces;
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
    }
}
