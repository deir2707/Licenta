using System.Threading.Tasks;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public interface IUserService
    {
        
        public Task<int> Register(RegisterInput registerInput);
        public Task<UserDetails> GetUserDetails(int id);

        public Task<UserDetails> Login(LoginInput loginInput);
        public Task<int> AddBalance(AddBalanceInput addBalanceInput);
    }
}