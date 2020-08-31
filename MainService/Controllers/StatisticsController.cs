using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsProvider _statisticsProvider;

        public StatisticsController(IStatisticsProvider statisticsProvider)
        {
            _statisticsProvider = statisticsProvider;
        }

        /// <summary>
        /// Получение всех метрик сервиса статистики
        /// </summary>
        [HttpGet("get-all")]
        public async Task<string> GetAllStatistics()
        {
            var statisticsAsJson = await Task.Run(() => _statisticsProvider.GetStatistics());

            return statisticsAsJson;
        }

        /// <summary>
        /// Получение метрик сервиса статистики по определенному хосту
        /// </summary>
        [HttpGet("get-by-host")]
        public async Task<string> GetStatistics(string host)
        {
            var statisticsAsJson = await Task.Run(() => _statisticsProvider.GetStatistics(host));

            return statisticsAsJson;
        }

        /// <summary>
        /// Получение метрик сервиса статистики по определенному хосту и методу
        /// </summary>
        [HttpGet("get-by-host-and-method")]
        public async Task<string> GetStatistics(string host, string method)
        {
            var statisticsAsJson = await Task.Run(() => _statisticsProvider.GetStatistics(host, method));

            return statisticsAsJson;
        }
    }
}