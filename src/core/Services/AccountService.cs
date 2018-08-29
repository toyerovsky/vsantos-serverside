using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using VRP.BLL.Dto;
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
            return _mapper.Map<IEnumerable<AccountModel>, IEnumerable<AccountDto>>(await _unitOfWork.AccountsRepository.JoinAndGetAllAsync(expression));
        }

        public async Task<IEnumerable<AccountDto>> GetAllNoRelatedAsync(Func<AccountModel, bool> func)
        {
            return _mapper.Map<IEnumerable<AccountModel>, IEnumerable<AccountDto>>(await _unitOfWork.AccountsRepository.GetAllAsync(func));
        }

        public async Task<AccountDto> GetByIdAsync(int id)
        {
            return _mapper.Map<AccountModel, AccountDto>(await _unitOfWork.AccountsRepository.GetAsync(id));
        }

        public async Task<AccountDto> GetAsync(Func<AccountModel, bool> func)
        {
            return _mapper.Map<AccountModel, AccountDto>(await _unitOfWork.AccountsRepository.GetAsync(func));
        }

        public async Task<AccountDto> UpdateAsync(int id, AccountDto dto)
        {
            AccountModel model = await _unitOfWork.AccountsRepository.JoinAndGetAsync(id);
            _unitOfWork.AccountsRepository.BeginUpdate(model);
            _mapper.Map(dto, model);
            await _unitOfWork.SaveAsync();
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