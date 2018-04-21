/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.Core.Database.Models;
using VRP.Core.Repositories;

namespace VRP.vAPI.Game.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class AccountsController : Controller
    {
        private readonly AccountsRepository _accountsRepository = new AccountsRepository();

        [HttpGet]
        public JsonResult Get()
        {
            IEnumerable<AccountModel> accounts = _accountsRepository.GetAll();
            return Json(accounts);
        }
        
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            AccountModel account = _accountsRepository.Get(id);
            return Json(account);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]AccountModel value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}