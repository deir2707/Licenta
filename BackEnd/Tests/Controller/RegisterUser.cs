using System;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Controllers;
using Domain;
using FluentAssertions;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Inputs;
using Xunit;

namespace Tests.Controller
{
    [Trait("When trying to register a new user", "")]
    public class RegisterUser : BaseTest
    {
        private readonly Func<Task<IActionResult>> _validAction;
        private readonly Func<Task<IActionResult>> _invalidAction;

        public RegisterUser()
        {
            var userService = Service<IUserService>();
            var controller = new UsersController(userService);

            var validRegisterInput = new RegisterInput()
            {
                Email = "john@doe.com",
                FullName = "John Doe",
                Password = "12345678"
            };

            var invalidRegisterInput = new RegisterInput()
            {
                Email = "jane@doe.com",
                FullName = "Jane Doe",
                Password = "12345"
            };

            _validAction = async () => await controller.Register(validRegisterInput);
            _invalidAction = async () => await controller.Register(invalidRegisterInput);

            var user = new User
            {
                Email = "jane@doe.com",
                FullName = "Jane Doe",
                Password = "12345678"
            };

            AuctionContext.Users.Add(user);
            AuctionContext.SaveChanges();
        }

        [Fact(DisplayName = "with valid data")]
        public void CreateValidAuction()
        {
            var result = _validAction.Invoke().Result;
            result.Should().BeOfType<OkObjectResult>();
            AuctionContext.Users.Count(u => u.Email == "john@doe.com").Should().Be(1);
        }

        [Fact(DisplayName = "with invalid data")]
        public void CreateAuctionWithNoOtherDetails()
        {
            _invalidAction.Should().ThrowAsync<AuctionException>().WithMessage("User already exists");
            AuctionContext.Users.Count().Should().Be(1);
        }
    }
}