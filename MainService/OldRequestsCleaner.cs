using System;
using System.Threading;
using System.Threading.Tasks;
using MainService.Requests;

namespace MainService
{
    public class OldRequestsCleaner : IOldRequestsCleaner
    {
        private readonly IRequestsStorage _storage;

        private readonly long _maxRequestTimeInMilliseconds = 60 * 60 * 1000;
        private readonly int _frequencyOfFinishingOldRequests = 5 * 60 * 1000;

        public OldRequestsCleaner(IRequestsStorage storage)
        {
            _storage = storage;
        }

        public async Task MoveOldRequestsToFailedRequestsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                foreach (var (guid, request) in _storage.UnfinishedRequests)
                {
                    var requestStartTime = request.StartTimeInMilliseconds;
                    var isRequestOld = now - requestStartTime > _maxRequestTimeInMilliseconds;

                    if (!isRequestOld) continue;
                    if (!_storage.UnfinishedRequests.TryRemove(guid, out var oldRequest)) continue;

                    var failedRequest = new FailedRequest(
                        oldRequest.Host, oldRequest.Method, oldRequest.StartTimeInMilliseconds);
                    _storage.LostUdpPackets.Add(failedRequest);
                }

                await Task.Delay(_frequencyOfFinishingOldRequests, token);
            }
        }
    }
}