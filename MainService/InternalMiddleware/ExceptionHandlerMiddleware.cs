using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MainService.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MainService.InternalMiddleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IRequestsStorage _storage;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        private static readonly Dictionary<Type, HttpStatusCode>
            ExceptionTypeToStatusCode = new Dictionary<Type, HttpStatusCode>
            {
                {typeof(NotImplementedException), HttpStatusCode.NotImplemented},
                {typeof(NullReferenceException), HttpStatusCode.InternalServerError}
            };

        public ExceptionHandlerMiddleware(RequestDelegate next, IRequestsStorage storage,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _storage = storage;
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
                AddFailedRequestToStorage(context);
                await WriteExceptionToResponseAsync(context, ex);
            }
        }

        private void AddFailedRequestToStorage(HttpContext context)
        {
            var method = context.Request.Method;
            var url = $"{context.Request.Host}/{context.Request.Path}";
            var now = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            _storage.FailedHttpRequests.Add(new FailedRequest(method, url, now));
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