using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Inputs;
using Service.Outputs;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("add-user")]
        public async Task<int> AddUser(AddUserInput addUserInput)
        {
            var addedId = await _userService.AddUser(addUserInput);
            return addedId;
        }

        [HttpGet]
        public async Task<UserDTO> GetUser(int id)
        {
            return await _userService.GetUserDTO(id);
        }

    }
}