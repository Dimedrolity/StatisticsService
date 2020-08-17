using System.Threading;
using System.Threading.Tasks;

namespace MainService.Controllers
{
    public interface IUdpListener
    {
        public Task Listen(CancellationToken token);
    }
}