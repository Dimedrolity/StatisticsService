using System;
using System.Threading;
using System.Threading.Tasks;
using MainService.Controllers;

namespace MainService
{
    public class Maintenance : IMaintenance, IDisposable
    {
        private readonly IOldRequestsCleaner _requestsCleaner;
        private readonly IUdpListener _udpListener;

        private CancellationTokenSource _tokenSource;

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
            
            //TODO передавать токены в контроллеры (MetricsContr, RequestsContr)

            await Task.WhenAll(udpListenerTask, requestsCleanerTask);
        }

        public void Stop()
        {
            _tokenSource.Cancel();
        }

        public void Dispose()
        {
            Stop();
            _tokenSource.Dispose();
        }
    }
}