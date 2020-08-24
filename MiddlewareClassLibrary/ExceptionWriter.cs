using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace MiddlewareClassLibrary
{
    internal class ExceptionWriter
    {
        private static readonly Dictionary<Type, HttpStatusCode>
            ExceptionTypeToStatusCode = new Dictionary<Type, HttpStatusCode>
            {
                {typeof(NotImplementedException), HttpStatusCode.NotImplemented},
                {typeof(NullReferenceException), HttpStatusCode.InternalServerError}
            };

        public async Task WriteExceptionToResponseAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "text/plain;charset=UTF-8";

            var firstException = GetExceptionRecursive(exception);

            context.Response.StatusCode = (int)
                (ExceptionTypeToStatusCode.ContainsKey(firstException.GetType())
                    ? ExceptionTypeToStatusCode[firstException.GetType()]
                    : HttpStatusCode.InternalServerError);

            var exceptionAsJson = GetExceptionAsJson(context, firstException);
            await context.Response.WriteAsync(exceptionAsJson);
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

        private string GetExceptionAsJson(HttpContext context, Exception exception)
        {
            return JsonConvert.SerializeObject(new
            {
                Type = exception.GetType().ToString(),
                Message = exception.Message,
                Method = context.Request.Method,
                Url = $"{context.Request.Host}/{context.Request.Path}",
                Stack = exception.StackTrace
            }, Formatting.Indented);
        }
    }
}