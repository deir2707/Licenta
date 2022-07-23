using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public interface IAuctionService
    {
        public Task<int> CreateAuction(AuctionInput auctionInput);
        Task<PaginationOutput<AuctionOutput>> GetAllAuctionDetails(int page, int pageSize);
        Task<AuctionDetails> GetAuctionDetails(int id);
        Task<int> MakeBid(BidInput bidInput);
        Task<List<AuctionOutput>> GetMyAuctions();
        Task<List<AuctionOutput>> GetWonAuctions();
    }
}