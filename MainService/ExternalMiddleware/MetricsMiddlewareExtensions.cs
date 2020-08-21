using Microsoft.AspNetCore.Builder;

namespace MainService.ExternalMiddleware
{
    public static class MetricsMiddlewareExtensions
    {
        public static IApplicationBuilder UseMetrics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MetricsMiddleware>();
        }
    }
}