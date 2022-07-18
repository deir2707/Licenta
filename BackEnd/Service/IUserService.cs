using System.Threading.Tasks;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public interface IUserService
    {
        
        public Task<int> Register(RegisterInput registerInput);
        public UserDTO GetUserDTO(int id);

        public Task<UserDTO> Login(LoginInput loginInput);
    }
}