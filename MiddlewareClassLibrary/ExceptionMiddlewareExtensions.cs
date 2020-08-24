using Microsoft.AspNetCore.Builder;

namespace MiddlewareClassLibrary
{
    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseCustomExceptionHandlerWithSendingToStatisticsService(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddlewareWithSendingToStatisticsService>();
        }
    }
}