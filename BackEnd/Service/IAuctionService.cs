using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Inputs;
using Service.Outputs;

namespace Service
{
    public interface IAuctionService
    {
        public Task<int> CreateCarAuction(CarInput carInput);
        Task<List<AuctionDetails>> GetAllAuctions();
    }
}