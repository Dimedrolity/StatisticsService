using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MiddlewareClassLibrary
{
    internal class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        private readonly ExceptionWriter _exceptionWriter = new ExceptionWriter();

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());

                await _exceptionWriter.WriteExceptionToResponseAsync(context, ex);
            }
        }
    }
}