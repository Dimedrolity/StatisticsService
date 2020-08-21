using MainService.Controllers;
using MainService.Metrics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MiddlewareClassLibrary;

namespace MainService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRequestsStorage, RequestsStorage>();
            services.AddSingleton<IOldRequestsCleaner, OldRequestsCleaner>();

            services.AddSingleton<Metric, UnfinishedRequestsMetric>();
            services.AddSingleton<Metric, RequestsAverageTimeMetric>();
            services.AddSingleton<Metric, RequestsMinTimeMetric>();
            services.AddSingleton<Metric, RequestsMaxTimeMetric>();
            services.AddSingleton<Metric, RequestsMedianTimeMetric>();
            services.AddSingleton<Metric, RequestsWithErrorsMetric>();
            services.AddSingleton<Metric, LostUdpPacketsMetric>();
            services.AddSingleton<IMetricsProvider, MetricsProvider>();

            services.AddSingleton<IUdpConfig, UdpConfig>();
            services.AddSingleton<IUdpListener, UdpListener>();

            services.AddSingleton<IMaintenance, Maintenance>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionHandler();

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}