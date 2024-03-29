﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
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
using VRP.API.Model;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;

namespace VRP.API.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IEnumerable<Claim> claims = await _accountService.GetClaimsAsync(loginModel.Email, loginModel.PasswordHash);

            if (claims != null)
            {
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                claimsIdentity.AddClaims(claims);
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    new AuthenticationProperties()
                );

                return SignIn(claimsPrincipal, CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return NotFound(loginModel);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return SignOut(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            IEnumerable<AccountDto> accounts = await _accountService.GetAllNoRelatedAsync();
            return Json(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            AccountDto account = await _accountService.GetByIdAsync(id);

            if (account == null)
            {
                return NotFound(id);
            }

            return Json(account);
        }

        [AllowAnonymous]
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetAsync(string email)
        {
            AccountDto account = await _accountService.GetAsync(a => a.Email == email);

            if (account == null)
            {
                return NotFound(email);
            }
            
            return Json(account);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _accountService?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}