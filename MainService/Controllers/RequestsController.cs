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
            var host = Request.Form["host"];
            var path = Request.Form["path"];
            var method = Request.Form["method"];
            var startTime = Request.Form["time-as-milliseconds-from-unix-epoch"];
            var url = $"{host}/{path}";

            await Task.Run(() =>
            {
                _requestsCollector.SaveStartedRequest(method, url, long.Parse(startTime));
            });

            Console.WriteLine($"начал выполнение {method}-запрос {host}/{path}");
            Console.WriteLine($"время начала запроса = {startTime}");
            
            return Ok();
        }

        [HttpPost("request-finished")]
        public async Task<IActionResult> RequestFinished()
        {
            var host = Request.Form["host"];
            var path = Request.Form["path"];
            var method = Request.Form["method"];
            var finishTime = Request.Form["time-as-milliseconds-from-unix-epoch"];
            var url = $"{host}/{path}";

            await Task.Run(() =>
            {
                _requestsCollector.SaveFinishedRequest(method, url, long.Parse(finishTime));
            });

            Console.WriteLine($"закончил выполнение {method}-запрос {host}/{path}");
            Console.WriteLine($"время завершения запроса = {finishTime}");
            
            return Ok();
        }
    }
}