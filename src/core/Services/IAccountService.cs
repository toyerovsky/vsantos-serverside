using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRP.BLL.Dto;
using VRP.DAL.Database.Models.Account;

namespace VRP.BLL.Services
{
    public interface IAccountService : IDisposable
    {
        Task<IEnumerable<AccountDto>> GetAllAsync(Expression<Func<AccountModel, bool>> expression);
        Task<IEnumerable<AccountDto>> GetAllNoRelatedAsync(Func<AccountModel, bool> func);
        Task<AccountDto> GetByIdAsync(int id);
        Task<AccountDto> GetAsync(Func<AccountModel, bool> func);
        Task<AccountDto> UpdateAsync(int id, AccountDto dto);
        Task DeleteAsync(int id);
        Task<bool> ContainsAsync(int id);
    }
}