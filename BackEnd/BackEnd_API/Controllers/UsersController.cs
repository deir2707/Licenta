using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Inputs;
using Service.Outputs;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController: ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginInput loginInput)
        {
            var userDto = await _userService.Login(loginInput);
            return Ok(userDto);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterInput registerInput)
        {
            var addedId = await _userService.Register(registerInput);
            return Ok(addedId);
        }

        [HttpGet("{id:int}")]
        public Task<IActionResult> GetUser(int id)
        {
            var userDto = _userService.GetUserDTO(id);
            return Task.FromResult<IActionResult>(Ok(userDto));
        }

    }
}