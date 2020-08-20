using System;
using System.Threading;
using System.Threading.Tasks;
using MainService.Controllers;

namespace MainService
{
    public class Maintenance : IMaintenance, IDisposable
    {
        public bool IsStopped => _tokenSource?.IsCancellationRequested ?? true;

        private CancellationTokenSource _tokenSource;

        private readonly IOldRequestsCleaner _requestsCleaner;
        private readonly IUdpListener _udpListener;

        public Maintenance(IOldRequestsCleaner requestsCleaner, IUdpListener udpListener)
        {
            _requestsCleaner = requestsCleaner;
            _udpListener = udpListener;
        }

        public async Task Start()
        {
            _tokenSource = new CancellationTokenSource();

            var udpListenerTask = _udpListener.Listen(_tokenSource.Token);

            var requestsCleanerTask = _requestsCleaner.MoveOldRequestsToFailedRequests(_tokenSource.Token);

            await Task.WhenAll(udpListenerTask, requestsCleanerTask);
        }

        public void Stop()
        {
            _tokenSource?.Cancel();
        }

        public void Dispose()
        {
            Stop();
            _tokenSource.Dispose();
        }
    }
}