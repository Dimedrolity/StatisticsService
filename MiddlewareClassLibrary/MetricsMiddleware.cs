using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MiddlewareClassLibrary
{
    internal class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IRequestSender _requestSender;

        public MetricsMiddleware(RequestDelegate next, IRequestSender requestSender)
        {
            _next = next;
            _requestSender = requestSender;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestGuid = (string) context.Items["guid"];

            var contentAboutStartedRequest = new Dictionary<string, string>
            {
                {"guid", requestGuid}, {"host", context.Request.Host.Value},
                {"path", context.Request.Path.Value}, {"method", context.Request.Method},
                {"start-time-as-milliseconds-from-unix-epoch", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()}
            };

            await _requestSender.SendStartedRequestAsync(contentAboutStartedRequest);

            await _next(context);

            var contentAboutFinishedRequest = new Dictionary<string, string>
            {
                {"guid", requestGuid},
                {"finish-time-as-milliseconds-from-unix-epoch", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()}
            };

            await _requestSender.SendFinishedRequestAsync(contentAboutFinishedRequest);
        }
    }
}