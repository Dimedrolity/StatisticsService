using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MainService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestsController : ControllerBase
    {
        private readonly IMaintenance _maintenance;
        private readonly IRequestsStorage _requestsStorage;
        private readonly ILogger<RequestsController> _logger;

        private static string ErrorMessage = "Сервис статистики остановлен";

        public RequestsController(IMaintenance maintenance, IRequestsStorage requestsStorage,
            ILogger<RequestsController> logger)
        {
            _maintenance = maintenance;
            _requestsStorage = requestsStorage;
            _logger = logger;
        }

        /// <summary>
        /// Добавление информации о начавшемся HTTP-запросе
        /// </summary>
        [HttpPost("request-started")]
        public async Task<IActionResult> RequestStarted(
            [FromForm(Name = "guid")] string guid,
            [FromForm(Name = "host")] string host,
            [FromForm(Name = "method")] string method,
            [FromForm(Name = "start-time-as-milliseconds-from-unix-epoch")]
            string startTime)
        {
            if (_maintenance.IsStopped) return StatusCode(403, ErrorMessage);

            await Task.Run(() => { _requestsStorage.SaveStartedRequest(guid, host, method, long.Parse(startTime)); });

            _logger.LogInformation($"начал выполнение запрос: {guid} метод: {method} url: {host}\n" +
                                   $"время начала запроса: {startTime}");

            return Ok();
        }

        /// <summary>
        /// Добавление информации о HTTP-запросе, который завершился успешно
        /// </summary>
        [HttpPost("request-finished")]
        public async Task<IActionResult> RequestFinished(
            [FromForm(Name = "guid")] string guid,
            [FromForm(Name = "host")] string host,
            [FromForm(Name = "method")] string method,
            [FromForm(Name = "finish-time-as-milliseconds-from-unix-epoch")]
            string finishTime)
        {
            if (_maintenance.IsStopped) return StatusCode(403, ErrorMessage);

            await Task.Run(() => { _requestsStorage.SaveFinishedRequest(guid, host, method, long.Parse(finishTime)); });

            _logger.LogInformation($"выполнился запрос: {guid}\n" +
                                   $"время окончания запроса: {finishTime}");

            return Ok();
        }

        /// <summary>
        /// Добавление информации о HTTP-запросе, который завершился с ошибкой
        /// </summary>
        [HttpPost("request-failed")]
        public async Task<IActionResult> RequestFailed(
            [FromForm(Name = "guid")] string guid,
            [FromForm(Name = "host")] string host,
            [FromForm(Name = "method")] string method,
            [FromForm(Name = "fail-time-as-milliseconds-from-unix-epoch")]
            string failTime)
        {
            if (_maintenance.IsStopped) return StatusCode(403, ErrorMessage);

            await Task.Run(() => { _requestsStorage.SaveRequestWithError(guid, host, method, long.Parse(failTime)); });

            _logger.LogInformation($"запрос завершился с ошибкой {guid}\n" +
                                   $"время ошибки: {failTime}");

            return Ok();
        }
    }
}