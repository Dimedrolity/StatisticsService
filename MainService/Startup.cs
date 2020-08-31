using System.IO;
using MainService.Controllers;
using MainService.Metrics;
using MainService.Requests;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
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

            services.AddSingleton<Metric<UnfinishedRequest>, UnfinishedRequestsCountMetric>();
            services.AddSingleton<Metric<FinishedRequest>, FinishedRequestsCountMetric>();
            services.AddSingleton<Metric<FinishedRequest>, RequestsAverageTimeMetric>();
            services.AddSingleton<Metric<FinishedRequest>, RequestsMinTimeMetric>();
            services.AddSingleton<Metric<FinishedRequest>, RequestsMaxTimeMetric>();
            services.AddSingleton<Metric<FinishedRequest>, RequestsMedianTimeMetric>();
            services.AddSingleton<Metric<FailedRequest>, RequestsWithErrorsCountMetric>();
            services.AddSingleton<Metric<FailedRequest>, LostUdpPacketsCountMetric>();
            services.AddSingleton<IMetricsProvider, MetricsProvider>();
            services.AddSingleton<IRequestsProvider, RequestsProvider>();
            services.AddSingleton<IStatisticsProvider, StatisticsProvider>();

            services.AddSingleton<IUdpConfig, UdpConfig>();
            services.AddSingleton<IUdpListener, UdpListener>();

            services.AddSingleton<IMaintenance, Maintenance>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Statistics service API",
                        Version = "v1"
                    }
                );

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "StatisticsServiceAPI.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomExceptionHandler();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = string.Empty;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Statistics service API");
                }
            );

            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}