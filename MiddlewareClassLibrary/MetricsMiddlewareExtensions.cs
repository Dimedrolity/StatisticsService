using Microsoft.AspNetCore.Builder;

namespace MiddlewareClassLibrary
{
    public static class MetricsMiddlewareExtensions
    {
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MetricsMiddleware>();
        }
    }
}