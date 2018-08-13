using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VRP.Core.Interfaces;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Enums;
using VRP.vAPI.UnitOfWork;

namespace VRP.vAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class SeedController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper<ServerRank, long> _serverRankMapper;

        public SeedController(IConfiguration configuration, IUnitOfWork unitOfWork, IMapper<ServerRank, long> serverRankMapper)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
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
                        _unitOfWork.AccountsRepository.Insert(account);
                    }
                    _unitOfWork.AccountsRepository.Save();
                }
            }
            return Ok();
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