using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VRP.Core.Database.Models.Account;
using VRP.Core.Enums;
using VRP.Core.Repositories;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SeedController : Controller
    {
        private readonly IConfiguration _configuration;

        public SeedController(IConfiguration configuration)
        {
            _configuration = configuration;
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
                using (AccountsRepository accountsRepository = new AccountsRepository())
                using (var multiple = connection.QueryMultiple(query))
                {
                    foreach (var account in multiple.Read<AccountModel>())
                    {
                        account.ServerRank = (ServerRank)account.PrimaryForumGroup;
                        accountsRepository.Insert(account);
                    }
                    accountsRepository.Save();
                }
            }
            return Ok();
        }
    }
}