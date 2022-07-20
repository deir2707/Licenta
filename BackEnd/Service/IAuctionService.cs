using System.Threading.Tasks;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public interface IAuctionService
    {
        public Task<int> CreateCarAuction(CarInput carInput);
        Task<PaginationOutput<AuctionOutput>> GetAllAuctionDetails(int page, int pageSize);
        Task<AuctionDetails> GetAuctionDetails(int id);
        Task<int> MakeBid(BidInput bidInput);
    }
}