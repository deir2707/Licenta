using System;
using System.Threading.Tasks;
using Service.Inputs;

namespace Service
{
    public class AuctionService:IAuctionService
    {
        public bool CreateCarAction(CarInput carInput)
        {
            
            
            Console. WriteLine(carInput.Description+" "+carInput.Make);
            return true;
        }
    }
}