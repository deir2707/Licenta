using System;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.Extensions;
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

        public async Task<int> Register(RegisterInput registerInput)
        {
            var user = await FetchUserByEmail(registerInput.Email);

            if (user is not null)
                throw new AuctionException(ErrorCode.User_Already_Exists, "User already exists");
                    
            user = new User
            {
                Email = registerInput.Email,
                Password = registerInput.Password
            };
            
            var x=await _userRepository.AddAsync(user);
            await _userRepository.SaveChanges();
            
            return x.Id;
        }

        public async Task<UserDTO> GetUserDTO(int id)
        {
            var user = await GetUser(id);

            return user.ToUserDto();
        }

        private async Task<User> GetUser(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user is null)
            {
                throw new AuctionException(ErrorCode.User_Not_Found, "Invalid id");
            }

            return user;
        }
        private async Task<User> GetUserByEmail(string email)
        {
            var user = await FetchUserByEmail(email);
            
            if (user is null)
            {
                throw new AuctionException(ErrorCode.User_Not_Found, "Invalid credentials");
            }

            return user;
        }

        private async Task<User> FetchUserByEmail(string email)
        {
            return await _userRepository.DbContext.Users
                .FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<UserDTO> Login(LoginInput loginInput)
        {
            var user = await GetUserByEmail(loginInput.Email);
            if (!user.Password.Equals(loginInput.Password))
                throw new AuctionException(ErrorCode.User_Not_Found, "Invalid credentials");
            return user.ToUserDto();
        }
    }
}