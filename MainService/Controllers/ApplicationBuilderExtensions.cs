using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MainService.Controllers
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> StartMaintenance(this IApplicationBuilder app)
        {
            var maintenance = app.ApplicationServices.GetService<IMaintenance>();
            await Task.Run(async () => { await maintenance.Start(); });
            return app;
        }
    }
}