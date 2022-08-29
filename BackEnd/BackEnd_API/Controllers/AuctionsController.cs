using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Inputs;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionsController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpPost("add-auction")]
        public async Task<IActionResult> CreateAuction([FromForm] AuctionInput auctionInput)
        {
            var response = await _auctionService.CreateAuction(auctionInput);
            return Ok(response);
        }

        [HttpGet("{page:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAllAuctions(int page, int pageSize)
        {
            var response = await _auctionService.GetAllAuctionDetails(page, pageSize);
            return Ok(response);
        }

        [HttpGet("my-auctions")]
        public async Task<IActionResult> GetMyAuctions()
        {
            var response = await _auctionService.GetMyAuctions();
            return Ok(response);
        }

        [HttpGet("won-auctions")]
        public async Task<IActionResult> GetWonAuctions()
        {
            var response = await _auctionService.GetWonAuctions();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllAuctions(Guid id)
        {
            var response = await _auctionService.GetAuctionDetails(id);
            return Ok(response);
        }

        [HttpPost("make-bid")]
        public async Task<IActionResult> MakeBid(BidInput bidInput)
        {
            var id = await _auctionService.MakeBid(bidInput.AuctionId, bidInput.BidAmount);
            return Ok(id);
        }
    }
}