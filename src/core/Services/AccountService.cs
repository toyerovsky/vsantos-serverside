using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using VRP.BLL.Dto;
using VRP.BLL.Services.Interfaces;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.UnitOfWork;

namespace VRP.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAsync(Expression<Func<AccountModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<AccountModel>, AccountDto[]>(await _unitOfWork.AccountsRepository.JoinAndGetAllAsync(expression));
        }

        public async Task<IEnumerable<AccountDto>> GetAllNoRelatedAsync(Expression<Func<AccountModel, bool>> expression)
        {
            return _mapper.Map<IEnumerable<AccountModel>, AccountDto[]>(await _unitOfWork.AccountsRepository.GetAllAsync(expression));
        }

        public async Task<AccountDto> GetByIdAsync(int id)
        {
            return _mapper.Map<AccountModel, AccountDto>(await _unitOfWork.AccountsRepository.JoinAndGetAsync(id));
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(string userEmail, string passwordHash)
        {
            AccountModel accountModel = await _unitOfWork.AccountsRepository.GetAsync(account => account.Email == userEmail);

            if (accountModel != null && accountModel.PasswordHash == passwordHash)
            {
                IEnumerable<Claim> claims = new List<Claim>
                {
                    new Claim("AccountId", accountModel.Id.ToString()),
                    new Claim(ClaimTypes.Email, accountModel.Email),
                    new Claim("ForumUserName", accountModel.ForumUserName),
                    new Claim(ClaimTypes.Role, ((int)accountModel.ServerRank).ToString())
                };
                return claims;
            }

            return null;
        }

        public async Task<AccountDto> GetAsync(Expression<Func<AccountModel, bool>> expression)
        {
            return _mapper.Map<AccountModel, AccountDto>(await _unitOfWork.AccountsRepository.JoinAndGetAsync(expression));
        }

        public async Task<AccountDto> UpdateAsync(int id, AccountDto dto)
        {
            AccountModel model = await _unitOfWork.AccountsRepository.JoinAndGetAsync(id);
            if (model != null)
            {
                _unitOfWork.AccountsRepository.BeginUpdate(model);
                _mapper.Map(dto, model);
                await _unitOfWork.SaveAsync();
            }
            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.AccountsRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<bool> ContainsAsync(int id)
        {
            return await _unitOfWork.AccountsRepository.ContainsAsync(id);
        }

        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }
    }
}