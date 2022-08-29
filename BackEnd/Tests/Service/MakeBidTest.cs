using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using FluentAssertions;
using Infrastructure.Models;
using Moq;
using Repository;
using Service;
using Xunit;

namespace Tests.Service
{
    [Trait("", "")]
    public class MakeBidTest : BaseTest
    {
        private Mock<IRepository<User>> _userRepositoryMock;
        private Mock<IRepository<Auction>> _auctionRepositoryMock;
        private Mock<IRepository<Bid>> _bidRepositoryMock;
        private IAuctionService _auctionService;

        private readonly Guid AuctionId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private readonly Guid UserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        private Auction _auction;
        private User _user;

        private Func<Task<Guid>> _validAction;
        private Func<Task<Guid>> _invalidAction;
        
        public MakeBidTest()
        {
            SetupDbContext();
            SetupMocks();
        }

        private void SetupMocks()
        {
            _auctionRepositoryMock = new Mock<IRepository<Auction>>();
            _auctionRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<Expression<Func<Auction,object>>[]?>()))
                .Returns(Task.FromResult(_auction));

            _userRepositoryMock = new Mock<IRepository<User>>();
            _bidRepositoryMock = new Mock<IRepository<Bid>>();

            _auctionService = new AuctionService(NotificationPublisherMock.Object, CurrentUserProviderMock.Object,
                _auctionRepositoryMock.Object, _userRepositoryMock.Object, _bidRepositoryMock.Object);
            
            _validAction = async () => await _auctionService.MakeBid(AuctionId, 101);
            _invalidAction = async () => await _auctionService.MakeBid(AuctionId, 100);
        }

        private void SetupDbContext()
        {
            _user = new User()
            {
                Id = UserId,
                FullName = "John Doe",
                Balance = 10000,
                Email = "john@doe.com",
                Password = "12345678"
            };

            AuctionContext.Users.Add(_user);
            AuctionContext.SaveChanges();

            CurrentUserProviderMock.Setup(x => x.UserId).Returns(UserId);
            CurrentUserProviderMock.Setup(x => x.User).Returns(_user);

            _auction = new Auction
            {
                Id = AuctionId,
                Title = "Test Auction",
                Description = "Test Auction Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                SellerId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                StartingPrice = 100,
                OtherDetails = new Dictionary<string, string>
                {
                    {"Make", "Make"},
                    {"Model", "Make"},
                    {"Year", "Make"}
                }
            };

            AuctionContext.Auctions.Add(_auction);
            AuctionContext.SaveChanges();
        }

        [Fact]
        public void MakeValidBid()
        {
            _validAction.Should().NotThrowAsync();
        }

        [Fact]
        public void MakeInvalidBid()
        {
            _invalidAction.Should().ThrowAsync<AuctionException>().WithMessage("Bid amount must be greater than the starting bid");
        }
    }
}