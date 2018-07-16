﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.Core.Extensions;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Interfaces;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Penalty")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class PenaltyController : Controller
    {
        private readonly IJoinableRepository<PenaltyModel> _penaltiesRepository;

        public PenaltyController(IJoinableRepository<PenaltyModel> penaltiesRepository)
        {
            _penaltiesRepository = penaltiesRepository;
        }

        // GET: api/Penalty
        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<PenaltyModel> penalties = _penaltiesRepository.GetAll();
            return Json(penalties);
        }

        [HttpGet("account/{id}")]
        public IActionResult GetByAccountId(int id)
        {
            IEnumerable<PenaltyModel> penalties =
                _penaltiesRepository.JoinAndGetAll().Where(penalty => penalty.Account.Id == id);
            return Json(penalties.Select(penalty => new
            {
                date = penalty.Date,
                expiryDate = penalty.ExpiryDate,
                penaltyType = penalty.PenaltyType.GetDescription(),
                reason = penalty.Reason,
                creator = new
                {
                    forumUserName = penalty.Creator.ForumUserName,
                    serverRank = penalty.Creator.ServerRank.GetDescription()
                }
            }));
        }

        // GET: api/Penalty/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Penalty
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Penalty/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}