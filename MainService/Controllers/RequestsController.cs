using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestsCollector _requestsCollector;

        public RequestsController(IRequestsCollector requestsCollector)
        {
            _requestsCollector = requestsCollector;
        }

        [HttpPost("request-started")]
        public async Task<IActionResult> RequestStarted()
        {
            var guid = Request.Form["guid"];
            var host = Request.Form["host"];
            var path = Request.Form["path"];
            var method = Request.Form["method"];
            var startTime = Request.Form["time-as-milliseconds-from-unix-epoch"];
            var url = $"{host}/{path}";

            await Task.Run(() => { _requestsCollector.SaveStartedRequest(guid, method, url, long.Parse(startTime)); });

            Console.WriteLine($"начал выполнение запрос: {guid} метод: {method} url: {host}/{path}");
            Console.WriteLine($"время начала запроса: {startTime}");

            return Ok();
        }

        [HttpPost("request-finished")]
        public async Task<IActionResult> RequestFinished()
        {
            var guid = Request.Form["guid"];
            var finishTime = Request.Form["time-as-milliseconds-from-unix-epoch"];

            await Task.Run(() => { _requestsCollector.SaveFinishedRequest(guid, long.Parse(finishTime)); });

            return Ok();
        }
    }
}