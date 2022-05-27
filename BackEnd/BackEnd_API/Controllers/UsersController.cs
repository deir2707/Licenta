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

        [HttpPost("login")]
        public async Task<UserDTO> LoginUser(LoginInput loginInput)
        {
            var userDto = await _userService.Login(loginInput);
            return userDto;
        }

        [HttpPost("register")]
        public async Task<int> Register(RegisterInput registerInput)
        {
            var addedId = await _userService.Register(registerInput);
            return addedId;
        }

        [HttpGet("{id:int}")]
        public async Task<UserDTO> GetUser(int id)
        {
            return await _userService.GetUserDTO(id);
        }

    }
}