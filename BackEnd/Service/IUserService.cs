using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public interface IUserService
    {
        public Task<UserDetails> Register(RegisterInput registerInput);
        public Task<UserDetails> GetUserDetails(Guid id);
        public Task<UserDetails> Login(LoginInput loginInput);
        public Task<Guid> AddBalance(AddBalanceInput addBalanceInput);
        Task<List<UserDetails>> GetAllUsers();
    }
}