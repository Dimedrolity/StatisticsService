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
        private readonly IRequestsStorage _storage;
        private readonly IMetricsProvider _metricsProvider;

        public MetricsController(IRequestsStorage storage, IMetricsProvider metricsProvider)
        {
            _storage = storage;
            _metricsProvider = metricsProvider;
        }

        /// <summary>
        /// Получение всех метрик сервиса статистики
        /// </summary>
        [HttpGet("get-all")]
        public async Task<string> GetAllMetrics()
        {
            var metricsAsJson = await Task.Run(() =>
            {
                var metrics = _metricsProvider.GetAllMetrics()
                    .ToDictionary(metric => metric.Name, metric => metric.GetValue(_storage));

                return JsonConvert.SerializeObject(metrics);
            });

            return metricsAsJson;
        }
    }
}