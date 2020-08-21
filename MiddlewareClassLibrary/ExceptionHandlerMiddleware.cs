using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MiddlewareClassLibrary
{
    internal class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        private static readonly Dictionary<Type, HttpStatusCode>
            ExceptionTypeToStatusCode = new Dictionary<Type, HttpStatusCode>
            {
                {typeof(NotImplementedException), HttpStatusCode.NotImplemented},
                {typeof(NullReferenceException), HttpStatusCode.InternalServerError}
            };

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
                await WriteExceptionToResponseAsync(context, ex);
            }
        }

        private async Task WriteExceptionToResponseAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "text/plain;charset=UTF-8";

            var firstException = GetExceptionRecursive(exception);
            context.Response.StatusCode = (int)
                (ExceptionTypeToStatusCode.ContainsKey(firstException.GetType())
                    ? ExceptionTypeToStatusCode[firstException.GetType()]
                    : HttpStatusCode.InternalServerError);

            var body = JsonConvert.SerializeObject(new
            {
                Type = firstException.GetType().ToString(),
                Message = firstException.Message,
                Method = context.Request.Method,
                Url = $"{context.Request.Host}/{context.Request.Path}",
                Stack = firstException.StackTrace
            }, Formatting.Indented);

            await context.Response.WriteAsync(body);
        }

        private Exception GetExceptionRecursive(Exception exception)
        {
            while (true)
            {
                if (exception.InnerException == null || ExceptionTypeToStatusCode.ContainsKey(exception.GetType()))
                    return exception;
                exception = exception.InnerException;
            }
        }
    }
}