using System;
using System.Threading.Tasks;
using Domain;
using Infrastructure;
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
        private ICurrentUserProvider _currentUserProvider;

        public UserService(IRepository<User> userRepository, ICurrentUserProvider currentUserProvider)
        {
            _userRepository = userRepository;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<int> Register(RegisterInput registerInput)
        {
            var user = await FetchUserByEmail(registerInput.Email);

            if (user is not null)
                throw new AuctionException(ErrorCode.UserAlreadyExists, "User already exists");
                    
            user = new User
            {
                Email = registerInput.Email,
                Password = registerInput.Password
            };
            
            var x=await _userRepository.AddAsync(user);
            await _userRepository.SaveChanges();
            
            return x.Id;
        }

        public UserDetails GetUserDTO(int id)
        {
            var user = _currentUserProvider.User;
            return user.ToUserDetails();
        }

        private async Task<User> FetchUserByEmail(string email)
        {
            return await _userRepository.DbContext.Users
                .FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<UserDetails> Login(LoginInput loginInput)
        {
            var user = await FetchUserByEmail(loginInput.Email);
            
            ValidateUser(user);
            
            if (!user.Password.Equals(loginInput.Password))
                throw new AuctionException(ErrorCode.UserNotFound, "Invalid credentials");
            
            return user.ToUserDetails();
        }

        private void ValidateUser(User user)
        {
            if (user is null)
            {
                throw new AuctionException(ErrorCode.UserNotFound, "Invalid credentials");
            }
        }
    }
}