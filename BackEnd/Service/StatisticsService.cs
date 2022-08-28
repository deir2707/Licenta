using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.Extensions;
using Service.Outputs;

namespace Service
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IRepository<Auction> _auctionRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Bid> _bidsRepository;

        public StatisticsService(ICurrentUserProvider currentUserProvider, IRepository<Auction> auctionRepository,
            IRepository<User> userRepository, IRepository<Bid> bidsRepository)
        {
            _currentUserProvider = currentUserProvider;
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
            _bidsRepository = bidsRepository;
        }

        public Task<StatisticsOutput> GetStatistics()
        {
            var auctions = _auctionRepository.AsQueryable(
                new Expression<Func<Auction, object>>[]
                {
                    x => x.Bids,
                    x => x.Images,
                    x => x.Buyer,
                    x => x.Seller,
                }).ToList();

            var users = _userRepository.AsQueryable().ToList();

            var bids = _bidsRepository.AsQueryable().ToList();

            var mostActiveSeller = GetMostActiveSeller(auctions, users, bids);

            var mostActiveBuyer = GetMostActiveBuyer(auctions, users, bids);

            var mostExpensiveItem = GetMostExpensiveItem(auctions, bids);

            var mostExpensiveItemSoldByCurrentUser = GetMostExpensiveItemSoldByCurrentUser(auctions, bids);

            var mostExpensiveItemBoughtByCurrentUser = GetMostExpensiveItemBoughtByCurrentUser(auctions, bids);

            return Task.FromResult(new StatisticsOutput
            {
                MostActiveSeller = mostActiveSeller,
                MostActiveBuyer = mostActiveBuyer,
                MostExpensiveItem = mostExpensiveItem.ToAuctionDetails(),
                MostExpensiveItemSoldByCurrentUser = mostExpensiveItemSoldByCurrentUser.ToAuctionDetails(),
                MostExpensiveItemBoughtByCurrentUser = mostExpensiveItemBoughtByCurrentUser.ToAuctionDetails(),
            });
        }

        private Auction GetMostExpensiveItem(List<Auction> auctions, List<Bid> bids)
        {
            var auctionsWithBuyer = auctions.Where(auction => auction.BuyerId.HasValue).ToList();

            if (!auctionsWithBuyer.Any())
                return null;

            var mostExpensive = FindMostExpensive(bids, auctionsWithBuyer);

            return mostExpensive;
        }

        private Auction GetMostExpensiveItemSoldByCurrentUser(List<Auction> auctions, List<Bid> bids)
        {
            var auctionsWithBuyer = auctions.Where(auction =>
                auction.BuyerId.HasValue && auction.SellerId == _currentUserProvider.UserId).ToList();

            if (!auctionsWithBuyer.Any())
                return null;

            var mostExpensive = FindMostExpensive(bids, auctionsWithBuyer);

            return mostExpensive;
        }

        private Auction GetMostExpensiveItemBoughtByCurrentUser(List<Auction> auctions, List<Bid> bids)
        {
            var auctionsWithBuyer = auctions.Where(auction => auction.BuyerId == _currentUserProvider.UserId).ToList();

            if (!auctionsWithBuyer.Any())
                return null;

            var mostExpensive = FindMostExpensive(bids, auctionsWithBuyer);

            return mostExpensive;
        }

        private static Auction FindMostExpensive(List<Bid> bids, List<Auction> auctions)
        {
            Auction mostExpensive = null;
            int mostExpensivePrice = 0;

            foreach (var auction in auctions)
            {
                var auctionBids = auction.Bids != null && auction.Bids.Count != 0
                    ? auction.Bids
                    : bids.Where(b => b.AuctionId == auction.Id).ToList();

                if (auctionBids.Count == 0) continue;

                var highestBid = auctionBids.Max(b=>b.Amount);
                if (highestBid > mostExpensivePrice)
                {
                    mostExpensive = auction;
                    mostExpensive.Bids = auctionBids;
                    mostExpensivePrice = highestBid;
                }
            }

            return mostExpensive;
        }

        private UserStatistics GetMostActiveSeller(List<Auction> auctions, List<User> users, List<Bid> bids)
        {
            if (!auctions.Any())
                return null;

            var auctionsGroupedBySeller = auctions.Where(a => a.BuyerId.HasValue).GroupBy(auction => auction.SellerId);

            var groupedBySeller = auctionsGroupedBySeller.ToList();
            if (!groupedBySeller.Any())
                return null;

            var mostActiveSellerGroup = groupedBySeller.OrderByDescending(g => g.Count()).First();

            var userAuctions = mostActiveSellerGroup.ToList();

            var money = userAuctions.Select(auction => auction.Bids != null && auction.Bids.Count != 0
                    ? auction.Bids
                    : bids.Where(b => b.AuctionId == auction.Id).ToList())
                .Select(auctionBids => auctionBids.Max(b => b.Amount))
                .Sum();

            var mostActiveSeller = users.First(u => u.Id == mostActiveSellerGroup.Key);

            return new UserStatistics
            {
                FullName = mostActiveSeller.FullName,
                Auctions = mostActiveSellerGroup.Count(),
                Money = money
            };
        }

        private UserStatistics GetMostActiveBuyer(List<Auction> auctions, List<User> users, List<Bid> bids)
        {
            var auctionsWithBuyer = auctions.Where(auction => auction.BuyerId.HasValue).ToList();

            if (!auctionsWithBuyer.Any())
                return null;

            var auctionsGroupedByBuyer = auctionsWithBuyer.GroupBy(auction => auction.BuyerId);

            var mostActiveBuyerGroup = auctionsGroupedByBuyer.OrderByDescending(g => g.Count()).First();

            var mostActiveBuyer = users.First(u => u.Id == mostActiveBuyerGroup.Key);

            var userAuctions = mostActiveBuyerGroup.ToList();

            var money = userAuctions.Select(auction => auction.Bids != null && auction.Bids.Count != 0
                    ? auction.Bids
                    : bids.Where(b => b.AuctionId == auction.Id).ToList())
                .Select(auctionBids => auctionBids.Max(b => b.Amount))
                .Sum();

            return new UserStatistics
            {
                FullName = mostActiveBuyer.FullName,
                Auctions = mostActiveBuyerGroup.Count(),
                Money = money
            };
        }
    }
}