using System.Threading;
using System.Threading.Tasks;

namespace MainService
{
    public interface IOldRequestsCleaner
    {
        Task MoveOldRequestsToFailedRequests(CancellationToken token);
    }
}