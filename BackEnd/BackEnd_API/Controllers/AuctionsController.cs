using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Inputs;
using Service.Outputs;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuctionsController
    {
        
        
            private readonly IAuctionService _auctionService;

            public AuctionsController(IAuctionService auctionService)
            {
                _auctionService = auctionService;
            }

            [HttpPost("add-car")]
            public async Task<Boolean> CreateCarAction( CarInput carInput)
            {
                var response =  _auctionService.CreateCarAction(carInput);
                return response;
            }
    }
}