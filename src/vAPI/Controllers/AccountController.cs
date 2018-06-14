/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VRP.Core.Database.Forum;
using VRP.Core.Database.Models.Account;
using VRP.Core.Enums;
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
        private readonly IJoinableRepository<AccountModel> _accountsRepository;
        private readonly IConfiguration _configuration;

        public AccountController(IJoinableRepository<AccountModel> accountsRepository,
            IConfiguration configuration)
        {
            _accountsRepository = accountsRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (IDbConnection connection =
                new MySqlConnection(_configuration.GetConnectionString("gameConnectionString")))
            {
                ForumDatabaseHelper forumDatabaseHelper =
                    new ForumDatabaseHelper(_configuration.GetConnectionString("forumConnectionString"));

                if (forumDatabaseHelper.CheckPasswordMatch(loginModel.Email, loginModel.Password, out ForumUser forumUser))
                {
                    string accountIdQuery = "SELECT Id FROM Accounts WHERE ForumUserId = @Id";
                    AccountModel account = connection.QuerySingleOrDefault<AccountModel>(accountIdQuery, new { forumUser.Id });

                    IEnumerable<Claim> claims = new List<Claim>()
                    {
                        new Claim("AccountId", account.Id.ToString()),
                        new Claim("CharacterId", ""),
                        new Claim(ClaimTypes.Name, forumUser.Email),
                        new Claim("ForumUserName", forumUser.UserName),
                        new Claim(ClaimTypes.Role, ((ServerRank) forumUser.GroupId).ToString()),
                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties authenticationProperties = new AuthenticationProperties();

                    ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        authenticationProperties);

                    return SignIn(principal, authenticationProperties, CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
            return NotFound();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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