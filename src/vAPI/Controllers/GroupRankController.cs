using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.DAL.Database.Models.Group;
using VRP.vAPI.Dto;
using VRP.vAPI.UnitOfWork;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class GroupRankController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GroupRankController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            GroupRankModel groupRank = _unitOfWork.GroupRanksRepository.Get(id);

            if (groupRank == null)
            {
                return NotFound(id);
            }

            GroupRankDto groupRankDto = _mapper.Map<GroupRankDto>(groupRank);
            return Json(groupRankDto);
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<GroupRankModel> groupRanks = _unitOfWork.GroupRanksRepository.GetAll();

            var groupRankModels = groupRanks as GroupRankModel[] ?? groupRanks.ToArray();

            if (!groupRankModels.Any())
            {
                return NotFound();
            }

            IEnumerable<GroupRankDto> groupRankDtos =
                _mapper.Map<GroupRankDto[]>(groupRankModels);

            return Json(groupRankDtos);
        }

        [HttpPost]
        public IActionResult Post([FromBody] GroupRankDto groupRankDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GroupRankModel groupRankModel = _mapper.Map<GroupRankModel>(groupRankDto);

            _unitOfWork.GroupRanksRepository.Insert(groupRankModel);
            _unitOfWork.GroupRanksRepository.Save();

            return Created("GET", groupRankModel);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] GroupRankDto groupRankDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(groupRankDto);
            }

            GroupRankModel groupRank = _unitOfWork.GroupRanksRepository.JoinAndGet(id);

            if (groupRank == null)
            {
                return NotFound(id);
            }

            _unitOfWork.GroupRanksRepository.BeginUpdate(groupRank);
            _mapper.Map(groupRankDto, groupRank);
            _unitOfWork.GroupRanksRepository.Save();

            return Json(_mapper.Map<GroupRankDto>(groupRank));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_unitOfWork.GroupRanksRepository.Contains(id))
            {
                return NotFound(id);
            }

            _unitOfWork.GroupRanksRepository.Delete(id);
            _unitOfWork.Save();

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
