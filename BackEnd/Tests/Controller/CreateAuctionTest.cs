using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Controllers;
using Domain;
using FluentAssertions;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Inputs;
using Xunit;
using Newtonsoft.Json;

namespace Tests.Controller
{
    [Trait("When trying to create auction", "")]
    public class CreateAuctionTest : BaseTest
    {
        private readonly Func<Task<IActionResult>> _validAction;
        private readonly Func<Task<IActionResult>> _invalidAction;

        public CreateAuctionTest()
        {
            var auctionService = Service<IAuctionService>();
            var controller = new AuctionsController(auctionService);

            var validAuctionInput = new AuctionInput
            {
                Title = "Title",
                Description = "Description",
                EndDate = DateTime.Now.AddDays(1),
                Images = new List<IFormFile>(),
                StartPrice = 100,
                Type = AuctionType.Car,
                OtherDetails = JsonConvert.SerializeObject(new Dictionary<string, string>
                {
                    {"Make", "Make"},
                    {"Model", "Make"},
                    {"Year", "Make"}
                })
            };

            var invalidAuctionInput = new AuctionInput
            {
                Title = "Title",
                Description = "Description",
                EndDate = DateTime.Now.AddDays(1),
                Images = new List<IFormFile>(),
                StartPrice = 100,
                Type = AuctionType.Car
            };

            _validAction = async () => await controller.CreateAuction(validAuctionInput);
            _invalidAction = async () => await controller.CreateAuction(invalidAuctionInput);
        }

        [Fact(DisplayName = "with valid data")]
        public void CreateValidAuction()
        {
            var result = _validAction.Invoke().Result;
            result.Should().BeOfType<OkObjectResult>();
            AuctionContext.Auctions.Count(a => a.Title == "Title").Should().Be(1);
        }

        [Fact(DisplayName = "with invalid data")]
        public void CreateAuctionWithNoOtherDetails()
        {
            _invalidAction.Should().ThrowAsync<AuctionException>().WithMessage("Auction has no details");
            AuctionContext.Auctions.Count().Should().Be(0);
        }
    }
}