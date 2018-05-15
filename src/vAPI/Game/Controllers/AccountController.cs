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
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Interfaces;
using VRP.Core.Repositories;
using VRP.Core.Services;
using VRP.vAPI.Game.Model;

namespace VRP.vAPI.Game.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class AccountController : Controller
    {
        private readonly IRepository<AccountModel> _accountsRepository;
        private readonly IUsersStorageService _usersStorageService;
        private readonly IUserBroadcasterService _userBroadcasterService;

        public AccountController(IRepository<AccountModel> accountsRepository, IUsersStorageService usersStorageService,
            IUserBroadcasterService userBroadcasterService)
        {
            _accountsRepository = accountsRepository;
            _usersStorageService = usersStorageService;
            _userBroadcasterService = userBroadcasterService;
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
                return Unauthorized();
            }

            ForumDatabaseHelper forumDatabaseHelper = new ForumDatabaseHelper(Startup.Configuration);
            if (forumDatabaseHelper.CheckPasswordMatch(loginModel.Email, loginModel.Password, out ForumUser forumUser))
            {
                Guid userGuid = Guid.NewGuid();
                Task.Run(() =>
                {
                    using (AccountsRepository accountsRepository = new AccountsRepository())
                    {
                        AccountModel accountModel = accountsRepository.GetNoRelated(account => account.ForumUserId == forumUser.Id);
                        _usersStorageService.Login(userGuid, accountModel.Id);
                        _userBroadcasterService.Broadcast(accountModel.Id, -1, userGuid, BroadcasterActionType.LogIn);
                    }
                });
                return Json(new { userGuid, accountId = _accountsRepository.GetNoRelated(account => account.ForumUserId == forumUser.Id).Id });
            }

            return NotFound();
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
    }
}