using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Account;

namespace VRP.BLL.Services.Interfaces
{
    public interface IAccountService : IDisposable
    {
        Task<IEnumerable<AccountDto>> GetAllAsync(Expression<Func<AccountModel, bool>> expression = null);
        Task<IEnumerable<AccountDto>> GetAllNoRelatedAsync(Expression<Func<AccountModel, bool>> expression = null);
        Task<AccountDto> GetByIdAsync(int id);
        /// <summary>
        /// Method to get proper claims for user
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="passwordHash"></param>
        /// <returns>Claims for user, or null if user doesn't exist</returns>
        Task<IEnumerable<Claim>> GetClaimsAsync(string userEmail, string passwordHash);
        Task<AccountDto> GetAsync(Expression<Func<AccountModel, bool>> expression = null);
        Task<AccountDto> UpdateAsync(int id, AccountDto dto);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
    }
}