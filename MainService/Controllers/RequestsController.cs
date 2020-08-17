﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MainService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestsStorage _requestsStorage;
        private readonly ILogger<RequestsController> _logger;

        public RequestsController(IRequestsStorage requestsStorage, ILogger<RequestsController> logger)
        {
            _requestsStorage = requestsStorage;
            _logger = logger;
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

            await Task.Run(() => { _requestsStorage.SaveStartedRequest(guid, method, url, long.Parse(startTime)); });

            _logger.LogInformation($"начал выполнение запрос: {guid} метод: {method} url: {host}/{path}\n" +
                                   $"время начала запроса: {startTime}");

            return Ok();
        }

        [HttpPost("request-finished")]
        public async Task<IActionResult> RequestFinished()
        {
            var guid = Request.Form["guid"];
            var finishTime = Request.Form["time-as-milliseconds-from-unix-epoch"];

            await Task.Run(() => { _requestsStorage.SaveFinishedRequest(guid, long.Parse(finishTime)); });

            _logger.LogInformation($"выполнился запрос: {guid}\n" +
                                   $"время окончания запроса: {finishTime}");

            return Ok();
        }
    }
}