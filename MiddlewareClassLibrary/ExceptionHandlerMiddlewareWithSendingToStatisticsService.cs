using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MiddlewareClassLibrary
{
    internal class ExceptionHandlerMiddlewareWithSendingToStatisticsService
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHandlerMiddlewareWithSendingToStatisticsService> _logger;
        private readonly ISender _sender;

        private readonly ExceptionWriter _exceptionWriter = new ExceptionWriter();

        public ExceptionHandlerMiddlewareWithSendingToStatisticsService(RequestDelegate next,
            ILogger<ExceptionHandlerMiddlewareWithSendingToStatisticsService> logger, ISender sender)
        {
            _next = next;
            _logger = logger;
            _sender = sender;
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
                {"time-as-milliseconds-from-unix-epoch", DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString()}
            };

            await _sender.SendFailedRequestAsync(contentAboutFailedRequest);
        }
    }
}