using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public interface IAuctionService
    {
        public Task<Guid> CreateAuction(AuctionInput auctionInput);
        Task<PaginationOutput<AuctionOutput>> GetAllAuctionDetails(int page, int pageSize);
        Task<AuctionDetails> GetAuctionDetails(Guid id);
        Task<Guid> MakeBid(Guid auctionId, int bidAmount);
        Task<List<AuctionOutput>> GetMyAuctions();
        Task<List<AuctionOutput>> GetWonAuctions();
    }
}