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

        public MetricsMiddleware(RequestDelegate next)
        {
            _next = next;
            _client = new HttpClient();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var urlForStartedRequest = "http://localhost:7000/api/requests/request-started";
            await SendRequestToStatisticsService(urlForStartedRequest);

            await _next(context);

            var urlForFinishedRequest = "http://localhost:7000/api/requests/request-finished";
            await SendRequestToStatisticsService(urlForFinishedRequest);

            async Task SendRequestToStatisticsService(string url)
            {
                await _client.PostAsync(url,
                    new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("host", context.Request.Host.Value),
                        new KeyValuePair<string, string>("path", context.Request.Path.Value),
                        new KeyValuePair<string, string>("method", context.Request.Method),
                        new KeyValuePair<string, string>("time-as-milliseconds-from-unix-epoch",
                            DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()),
                    }));
            }
        }
    }
}