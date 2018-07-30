using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Database.Models.Item;
using VRP.vAPI.Dto;
using VRP.vAPI.Extensions;
using VRP.vAPI.UnitOfWork;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class GroupController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GroupController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("account/{id}")]
        public IActionResult GetGoupsByAccounttId(int id)
        {
            List<WorkerModel> workers = new List<WorkerModel>();
            foreach (CharacterModel character in _unitOfWork.AccountsRepository.JoinAndGet(id).Characters)
            {
                workers.AddRange(character.Workers.ToList());
            }

            if (!workers.Any())
            {
                return NotFound(id);
            }

            IEnumerable<WorkerDto> workerDtos = _mapper.Map<WorkerDto[]>(workers);
            return Json(workerDtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            GroupModel group = _unitOfWork.GroupsRepository.JoinAndGet(id);

            if (group == null)
            {
                return NotFound(id);
            }

            GroupDto groupDto = _mapper.Map<GroupDto>(group);
            return Json(groupDto);
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