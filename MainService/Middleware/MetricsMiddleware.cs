using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MainService.Middleware
{
    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _client;

        private readonly string _urlForStartedRequest = "http://localhost:7000/api/requests/request-started";
        private readonly string _urlForFinishedRequest = "http://localhost:7000/api/requests/request-finished";

        public MetricsMiddleware(RequestDelegate next)
        {
            _next = next;
            _client = new HttpClient();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestGuid = Guid.NewGuid().ToString();

            var contentAboutStartedRequest = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"guid", requestGuid}, {"host", context.Request.Host.Value},
                {"path", context.Request.Path.Value}, {"method", context.Request.Method},
                {"time-as-milliseconds-from-unix-epoch", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()}
            });

            await _client.PostAsync(_urlForStartedRequest, contentAboutStartedRequest);

            await _next(context);

            var contentAboutFinishedRequest = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"guid", requestGuid},
                {"time-as-milliseconds-from-unix-epoch", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()}
            });

            await _client.PostAsync(_urlForFinishedRequest, contentAboutFinishedRequest);
        }
    }
}