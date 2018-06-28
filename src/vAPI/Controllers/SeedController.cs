using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Enums;
using VRP.DAL.Interfaces;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SeedController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<AccountModel> _accountsRepository;

        public SeedController(IConfiguration configuration, IRepository<AccountModel> accountsRepository)
        {
            _configuration = configuration;
            _accountsRepository = accountsRepository;
        }

        [HttpPost("seedaccounts")]
        public IActionResult Post()
        {
            var query = "SELECT member_id as ForumUserId," +
                        " name as Name," +
                        " member_group_id as PrimaryForumGroup,`" +
                        " email as Email FROM core_members";

            using (IDbConnection connection = new MySqlConnection(
                _configuration.GetConnectionString("forumConnectionString")))
            {
                using (var multiple = connection.QueryMultiple(query))
                {
                    foreach (var account in multiple.Read<AccountModel>())
                    {
                        account.ServerRank = (ServerRank)account.PrimaryForumGroup;
                        _accountsRepository.Insert(account);
                    }
                    _accountsRepository.Save();
                }
            }
            return Ok();
        }
    }
}