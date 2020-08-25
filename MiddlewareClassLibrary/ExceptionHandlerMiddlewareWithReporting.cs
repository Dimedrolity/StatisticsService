using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MiddlewareClassLibrary
{
    internal class ExceptionHandlerMiddlewareWithReporting
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHandlerMiddlewareWithReporting> _logger;
        private readonly IRequestSender _requestSender;

        private readonly ExceptionWriter _exceptionWriter = new ExceptionWriter();

        public ExceptionHandlerMiddlewareWithReporting(RequestDelegate next,
            ILogger<ExceptionHandlerMiddlewareWithReporting> logger, IRequestSender requestSender)
        {
            _next = next;
            _logger = logger;
            _requestSender = requestSender;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Items["guid"] = Guid.NewGuid().ToString();

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                await _exceptionWriter.WriteExceptionToResponseAsync(context, ex);

                await SendExceptionToStatisticsServiceAsync(context);
            }
        }

        private async Task SendExceptionToStatisticsServiceAsync(HttpContext context)
        {
            var contentAboutFailedRequest = new Dictionary<string, string>
            {
                {"guid", (string) context.Items["guid"]},
                {"fail-time-as-milliseconds-from-unix-epoch", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()}
            };

            await _requestSender.SendFailedRequestAsync(contentAboutFailedRequest);
        }
    }
}