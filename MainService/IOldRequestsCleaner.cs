using System.Threading;
using System.Threading.Tasks;

namespace MainService
{
    public interface IOldRequestsCleaner
    {
        public Task MoveOldRequestsToFailedRequests(CancellationToken token);
    }
}