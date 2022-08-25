using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            var response = await _statisticsService.GetStatistics();
            return Ok(response);
        }

    }
}