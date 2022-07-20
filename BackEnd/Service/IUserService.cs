using System.Threading.Tasks;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public interface IUserService
    {
        
        public Task<int> Register(RegisterInput registerInput);
        public UserDetails GetUserDTO(int id);

        public Task<UserDetails> Login(LoginInput loginInput);
    }
}