using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MainService.Middleware
{
    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IRequestSender _httpSender = new HttpSender();
        private readonly IRequestSender _udpSender = new UdpSender();

        public MetricsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestGuid = Guid.NewGuid().ToString();

            var contentAboutStartedRequest = new Dictionary<string, string>
            {
                {"guid", requestGuid}, {"host", context.Request.Host.Value},
                {"path", context.Request.Path.Value}, {"method", context.Request.Method},
                {"time-as-milliseconds-from-unix-epoch", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()}
            };

            await _udpSender.SendStartedRequest(contentAboutStartedRequest);

            await _next(context);

            var contentAboutFinishedRequest = new Dictionary<string, string>
            {
                {"guid", requestGuid},
                {"time-as-milliseconds-from-unix-epoch", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()}
            };

            await _udpSender.SendFinishedRequest(contentAboutFinishedRequest);
        }
    }
}