/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.Core.Repositories;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class AccountsController : Controller
    {
        private readonly AccountsRepository _accountsRepository = new AccountsRepository();

        //[HttpPost("login/{email}/{passwordHash}")]
        //public JsonResult Login(string email, string passwordHash)
        //{

        //}

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _accountsRepository.Dispose();
        }
    }
}