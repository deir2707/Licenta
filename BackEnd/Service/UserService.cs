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
        private readonly AuctionContext _auctionContext;
        private readonly ICurrentUserProvider _currentUserProvider;

        public UserService(AuctionContext auctionContext,
            ICurrentUserProvider currentUserProvider)
        {
            _auctionContext = auctionContext;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<int> Register(RegisterInput registerInput)
        {
            var user = await FetchUserByEmail(registerInput.Email);

            if (user is not null)
                throw new AuctionException(ErrorCode.UserAlreadyExists, "User already exists");

            var newUser = new User
            {
                Email = registerInput.Email,
                Password = registerInput.Password
            };

            await _auctionContext.AddAsync(newUser);
            await _auctionContext.SaveChangesAsync();

            return newUser.Id;
        }

        public async Task<UserDetails> GetUserDetails(int id)
        {
            var user = await _auctionContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            return user.ToUserDetails();
        }

        private async Task<User> FetchUserByEmail(string email)
        {
            return await _auctionContext.Users
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

        public async Task<int> AddBalance(AddBalanceInput addBalanceInput)
        {
            var user = await _auctionContext.Users.FirstAsync(x => x.Id == _currentUserProvider.UserId);
            user.Balance += addBalanceInput.BalanceToAdd;
            await _auctionContext.SaveChangesAsync();
            return user.Id;
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