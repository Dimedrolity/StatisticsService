using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MainService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly IRequestsCollector _collector;
        private readonly IMetricsProvider _metricsProvider;

        public MetricsController(IRequestsCollector collector, IMetricsProvider metricsProvider)
        {
            _collector = collector;
            _metricsProvider = metricsProvider;
        }

        [HttpGet("get-all")]
        public async Task<string> GetAllMetrics()
        {
            var metricsAsJson = await Task.Run(() =>
            {
                var metrics = _metricsProvider.GetAllMetrics()
                    .ToDictionary(metric => metric.Name, metric => metric.GetValue(_collector));

                return JsonConvert.SerializeObject(metrics);
            });

            return metricsAsJson;
        }
    }
}