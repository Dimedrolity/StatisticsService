using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MainService.Controllers
{
    public static class UdpListenerExtensions
    {
        public static async Task<IApplicationBuilder> UseUdpListener(this IApplicationBuilder builder)
        {
            var udpListener = builder.ApplicationServices.GetService<UdpListener>();
            await Task.Run(async () => { await udpListener.Listen(); });
            return builder;
        }
    }
}