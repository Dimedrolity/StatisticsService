using System.Threading;
using System.Threading.Tasks;

namespace MainService.Controllers
{
    public interface IUdpListener
    {
        Task Listen(CancellationToken token);
    }
}