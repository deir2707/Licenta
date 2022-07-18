using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Inputs;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuctionsController: ControllerBase
    {
            private readonly IAuctionService _auctionService;

            public AuctionsController(IAuctionService auctionService)
            {
                _auctionService = auctionService;
            }

            [HttpPost("add-car")]
            public async Task<IActionResult> CreateCarAction([FromForm] CarInput carInput)
            {
                var response = await  _auctionService.CreateCarAuction(carInput);
                return Ok(response);
            }
            
            [HttpGet]
            public async Task<IActionResult> GetAllAuctions()
            {
                var response = await _auctionService.GetAllAuctionDetails();
                return Ok(response);
            }
            
            [HttpPost("make-bid")]
            public async Task<IActionResult> MakeBid(BidInput bidInput)
            {
                var id = await _auctionService.MakeBid(bidInput);
                return Ok(id);
            }
    }
}