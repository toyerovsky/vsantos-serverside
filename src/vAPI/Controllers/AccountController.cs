/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.Core.Database.Forum;
using VRP.Core.Database.Models.Account;
using VRP.Core.Interfaces;
using VRP.Core.Repositories;
using VRP.Core.Services.UserStorage;
using VRP.vAPI.Model;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class AccountController : Controller
    {
        private readonly IRepository<AccountModel> _accountsRepository;
        private readonly IUsersStorageService _usersStorageService;

        public AccountController(IRepository<AccountModel> accountsRepository, IUsersStorageService usersStorageService)
        {
            _accountsRepository = accountsRepository;
            _usersStorageService = usersStorageService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_usersStorageService.IsUserOnline(loginModel.Email))
            {
                return Forbid("User is already on-line.");
            }

            ForumDatabaseHelper forumDatabaseHelper = new ForumDatabaseHelper(Startup.Configuration);
            if (forumDatabaseHelper.CheckPasswordMatch(loginModel.Email, loginModel.Password, out ForumUser forumUser))
            {
                Guid userGuid = Guid.NewGuid();
                AccountModel accountModel = _accountsRepository.Get(account => account.ForumUserId == forumUser.Id);
                Task.Run(() =>
                {
                    _usersStorageService.Login(userGuid, accountModel.Id);
                });
                return Json(new { userGuid, accountId = accountModel.Id });
            }

            return NotFound();
        }

        [HttpPost("logout")]
        public IActionResult LogOut([FromBody] Guid userGuid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_usersStorageService.IsUserOnline(userGuid))
            {
                _usersStorageService.LogOut(userGuid);
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<AccountModel> accounts = _accountsRepository.GetAll();
            return Json(accounts);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            AccountModel account = _accountsRepository.Get(id);
            return Json(account);
        }
    }
}