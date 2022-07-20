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
            
            [HttpGet("{page:int}/{pageSize:int}")]
            public async Task<IActionResult> GetAllAuctions(int page, int pageSize)
            {
                var response = await _auctionService.GetAllAuctionDetails(page, pageSize);
                return Ok(response);
            }
            
            [HttpGet("{id:int}")]
            public async Task<IActionResult> GetAllAuctions(int id)
            {
                var response = await _auctionService.GetAuctionDetails(id);
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