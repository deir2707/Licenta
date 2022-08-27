using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Inputs;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
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

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userDto = await _userService.GetUserDetails(id);
            return Ok(userDto);
        }

        [HttpPost("add-balance")]
        public async Task<IActionResult> AddBalance(AddBalanceInput addBalanceInput)
        {
            var addedId = await _userService.AddBalance(addBalanceInput);
            return Ok(addedId);
        }
    }
}