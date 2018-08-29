/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VRP.BLL.UnitOfWork;
using VRP.DAL.Database.Models.Account;
using VRP.vAPI.Dto;
using VRP.vAPI.Model;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("dev")]
    [Authorize("Authenticated")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AccountModel account = _unitOfWork.AccountsRepository.Get(a => a.Email == loginModel.Email);

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
            IEnumerable<AccountModel> accounts = _unitOfWork.AccountsRepository.GetAll();

            if (!accounts.Any())
            {
                return NotFound();
            }

            IEnumerable<AccountDto> accountDtos = _mapper.Map<AccountDto[]>(accounts);
            return Json(accountDtos);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            AccountModel account = _unitOfWork.AccountsRepository.Get(id);

            if (account == null)
            {
                return NotFound(id);
            }

            AccountDto accountDto = _mapper.Map<AccountDto>(account);
            return Json(accountDto);
        }

        [AllowAnonymous]
        [HttpGet("email/{email}")]
        public IActionResult Get(string email)
        {
            AccountModel account = _unitOfWork.AccountsRepository.Get(a => a.Email == email);

            if (account == null)
            {
                return NotFound(email);
            }

            AccountDto accountDto = _mapper.Map<AccountDto>(account);
            return Json(accountDto);
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