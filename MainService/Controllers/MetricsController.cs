using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly IMetrics _metrics;

        public MetricsController(Metrics metrics)
        {
            _metrics = metrics;
        }

        [HttpGet("unfinished-requests-count")]
        public async Task<IActionResult> GetRequestsCountInWork()
        {
            var unfinishedRequestsCount = _metrics.GetUnfinishedRequestsCount();
            return Ok(unfinishedRequestsCount);
        }

        [HttpGet("requests-average-time")]
        public async Task<IActionResult> GetRequestsAverageTime()
        {
            var requestsAverageTime = _metrics.GetRequestsAverageTime();
            return Ok(requestsAverageTime);
        }
    }
}