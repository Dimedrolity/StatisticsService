using System;
using System.Threading;
using System.Threading.Tasks;

namespace MainService.Controllers
{
    public class Maintenance : IMaintenance, IDisposable
    {
        private readonly IOldRequestsCleaner _requestsCleaner;
        private readonly IUdpListener _udpListener;

        private readonly CancellationTokenSource _tokenSource;

        public Maintenance(IOldRequestsCleaner requestsCleaner, IUdpListener udpListener)
        {
            _requestsCleaner = requestsCleaner;
            _udpListener = udpListener;
            _tokenSource = new CancellationTokenSource();
        }

        public async Task Start()
        {
            var token1 = _tokenSource.Token;
            var task1 = _udpListener.Listen(token1);

            var token2 = _tokenSource.Token;
            var task2 = _requestsCleaner.MoveOldRequestsToFailedRequests(token2);

            await Task.WhenAll(task1, task2);
        }

        public void Finish()
        {
            _tokenSource.Cancel();
        }

        public void Dispose()
        {
            Finish();
            _tokenSource.Dispose();
        }
    }
}