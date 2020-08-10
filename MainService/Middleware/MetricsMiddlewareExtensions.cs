using Microsoft.AspNetCore.Builder;

namespace MainService.Middleware
{
    public static class MetricsMiddlewareExtensions
    {
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MetricsMiddleware>();
        }
    }
}