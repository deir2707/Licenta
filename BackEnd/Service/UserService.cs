using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Models;
using Repository;
using Service.Extensions;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IRepository<User> _userRepository;

        public UserService(ICurrentUserProvider currentUserProvider,
            IRepository<User> userRepository)
        {
            _currentUserProvider = currentUserProvider;
            _userRepository = userRepository;
        }

        public async Task<UserDetails> Register(RegisterInput registerInput)
        {
            var user = FetchUserByEmail(registerInput.Email);

            if (user is not null)
                throw new AuctionException(ErrorCode.UserAlreadyExists, "User already exists");

            var newUser = new User
            {
                Email = registerInput.Email,
                Password = registerInput.Password,
                FullName = registerInput.FullName,
            };

            _currentUserProvider.SetUser(newUser);

            await _userRepository.InsertOneAsync(newUser);

            return newUser.ToUserDetails();
        }

        public async Task<UserDetails> GetUserDetails(Guid id)
        {
            var user = await _userRepository.FindByIdAsync(id);

            if (user is null)
                throw new AuctionException(ErrorCode.UserNotFound, "User not found");

            return user.ToUserDetails();
        }

        private User? FetchUserByEmail(string email)
        {
            return _userRepository.AsQueryable().FirstOrDefault(u => u.Email.Equals(email));
        }

        public Task<UserDetails> Login(LoginInput loginInput)
        {
            var user = FetchUserByEmail(loginInput.Email);

            if (user is null)
                throw new AuctionException(ErrorCode.UserNotFound, "Invalid credentials");

            if (!user.Password.Equals(loginInput.Password))
                throw new AuctionException(ErrorCode.UserNotFound, "Invalid credentials");

            return Task.FromResult(user.ToUserDetails());
        }

        public async Task<Guid> AddBalance(AddBalanceInput addBalanceInput)
        {
            var user = await _userRepository.FindByIdAsync(_currentUserProvider.UserId);

            if (user is null)
            {
                throw new AuctionException(ErrorCode.UserNotFound, "User not found");
            }

            user.Balance += addBalanceInput.BalanceToAdd;
            await _userRepository.ReplaceOneAsync(user);
            return user.Id;
        }

        public Task<List<UserDetails>> GetAllUsers()
        {
            var users = _userRepository.AsQueryable().ToList();

            return Task.FromResult(users.Select(u => u.ToUserDetails()).ToList());
        }
    }
}