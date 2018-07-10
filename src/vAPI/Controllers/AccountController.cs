/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Interfaces;
using VRP.vAPI.Model;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class AccountController : Controller
    {
        private readonly IJoinableRepository<AccountModel> _accountsRepository;

        public AccountController(IJoinableRepository<AccountModel> accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AccountModel account = _accountsRepository.Get(a => a.Email == loginModel.Email);

            if (account != null && account.PasswordHash == loginModel.PasswordHash)
            {
                IEnumerable<Claim> claims = new List<Claim>
                {
                    new Claim("AccountId", account.Id.ToString()),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim("ForumUserName", account.ForumUserName),
                    new Claim(ClaimTypes.Role, account.ServerRank.ToString())
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaims(claims);

                AuthenticationProperties authenticationProperties = new AuthenticationProperties();

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    authenticationProperties
                );

                return SignIn(claimsPrincipal, CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return NotFound(loginModel);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
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

        [HttpGet("{email}")]
        [AllowAnonymous]
        public IActionResult Get(string email)
        {
            AccountModel account = _accountsRepository.Get(a => a.Email == email);
            var loginModel = new
            {
                passwordSalt = account.PasswordSalt,
                forumUserName = account.ForumUserName
            };
            return Json(loginModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _accountsRepository?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}