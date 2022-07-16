using System.Threading.Tasks;
using Service.Inputs;

namespace Service
{
    public interface IAuctionService
    {
        public Task<int> CreateCarAuction(CarInput carInput);
    }
}