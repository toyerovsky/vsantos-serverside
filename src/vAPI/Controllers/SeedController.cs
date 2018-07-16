using System.Data;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VRP.Core.Interfaces;
using VRP.Core.Mappers;
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
        private readonly IJoinableRepository<AccountModel> _accountsRepository;
        private readonly IMapper<ServerRank, long> _serverRankMapper;

        public SeedController(IConfiguration configuration, IJoinableRepository<AccountModel> accountsRepository, IMapper<ServerRank, long> serverRankMapper)
        {
            _configuration = configuration;
            _accountsRepository = accountsRepository;
            _serverRankMapper = serverRankMapper;
        }

        [HttpPost("seedaccounts")]
        public IActionResult Post()
        {
            var query = "SELECT member_id as ForumUserId," +
                        " name as ForumUserName, members_pass_hash as PasswordHash," +
                        " member_group_id as PrimaryForumGroup, members_pass_salt as PasswordSalt, pp_main_photo as AvatarUrl," +
                        " email as Email, pp_gravatar as GravatarEmail FROM core_members";
            
            using (IDbConnection connection = new MySqlConnection(
                _configuration.GetConnectionString("forumConnectionString")))
            {
                using (var multiple = connection.QueryMultiple(query))
                {
                    foreach (var account in multiple.Read<AccountModel>())
                    {
                        account.ServerRank = _serverRankMapper.Map(account.PrimaryForumGroup);
                        _accountsRepository.Insert(account);
                    }
                    _accountsRepository.Save();
                }
            }
            return Ok();
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