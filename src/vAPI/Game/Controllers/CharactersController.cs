﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential
* Written by V Role Play team <contact@v-rp.pl> December 2017
*/

using System.Data;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VRP.Core.Database;
using VRP.Core.Database.Models;
using VRP.vAPI.Game.Services;

namespace VRP.vAPI.Game.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    public class CharacterController : Controller
    {
        private readonly IUsersWatcher _usersWatcher;
        private readonly IConfiguration _configuration;

        public CharacterController(IUsersWatcher usersWatcher, IConfiguration configuration)
        {
            _usersWatcher = usersWatcher;
            _configuration = configuration;
        }

        [HttpGet("account/{accountId}")]
        public IActionResult GetByAccountId(int accountId)
        {
            var query = "SELECT Name, Surname, Money FROM vrpsrv.Characters WHERE AccountId = @id;";

            using (IDbConnection connection = new MySqlConnection(
                _configuration.GetConnectionString("gameConnectionString")))
            {
                using (var multiple = connection.QueryMultiple(query, new { id = accountId }))
                {
                    var characters = multiple.Read<CharacterModel>().Select(character => new
                    {
                        name = character.Name,
                        surname = character.Surname,
                        money = character.Money,
                        lastLoginTime = character.LastLoginTime
                    });
                    return Json(characters);
                }
            }
        }
    }
}