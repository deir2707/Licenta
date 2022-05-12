using System;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public class UserService : IUserService
    {
        private IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<int> AddUser(AddUserInput addUserInput)
        {
            var user = new User
            {
                Email = addUserInput.Email,
                Password = addUserInput.Password
            };
            
            var x=await _userRepository.AddAsync(user);
            await _userRepository.SaveChanges();
            
            return x.Id;
        }

        public async Task<UserDTO> GetUserDTO(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user is null)
            {
                throw new Exception("Invalid id");
            }

            var userDTO = new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
            };
            return userDTO;
        }
    }
}