/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models;
using VRP.vAPI.Services;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class CharactersController : Controller
    {
        private readonly RoleplayContext _roleplayContext = RolePlayContextFactory.NewContext();
        private readonly IUsersWatcher _usersWatcher;

        public CharactersController(IUsersWatcher usersWatcher)
        {
            _usersWatcher = usersWatcher;
        }

        [HttpGet("{accountId}/account")]
        public JsonResult GetByAccountId(int accountId)
        {
            Stopwatch sw = new Stopwatch();
            sw.Reset();
            sw.Start();
            AccountModel account = _roleplayContext.Accounts
                .Single(a => a.Id == accountId);

            // loading alive characters
            _roleplayContext.Entry(account)
                .Collection(a => a.Characters)
                .Query()
                .Where(character => character.IsAlive)
                .Load();

            sw.Stop();
            Debug.WriteLine($"[RP DEBUG] {sw.ElapsedMilliseconds}");
            return Json(account.Characters.Select(character => new
            {
                name = character.Name,
                surname = character.Surname,
                money = character.Money
            }));
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _roleplayContext.Dispose();
        }
    }
}