using System.Threading.Tasks;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public interface IUserService
    {
        
        public Task<int> AddUser(AddUserInput addUserInput);
        public Task<UserDTO> GetUserDTO(int id);

    }
}