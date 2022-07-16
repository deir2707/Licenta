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
    }
}