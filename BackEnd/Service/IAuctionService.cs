using System;
using System.Threading.Tasks;
using Service.Inputs;

namespace Service
{
    public interface IAuctionService
    {
        public bool CreateCarAction(CarInput carInput);
    }
}