using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using MainService.Requests;

namespace MainService
{
    public class RequestsCollector : IRequestsCollector
    {
        public ConcurrentDictionary<string, UnfinishedRequest> UnfinishedRequests { get; }
        public ConcurrentBag<FinishedRequest> FinishedRequests { get; }

        private readonly long _maxRequestTimeInMilliseconds = 60 * 60 * 1000;
        private readonly int _frequencyOfFinishingOldRequests = 5 * 60 * 1000;

        public RequestsCollector()
        {
            UnfinishedRequests = new ConcurrentDictionary<string, UnfinishedRequest>();
            FinishedRequests = new ConcurrentBag<FinishedRequest>();

            Task.Run(FinishOldRequests);
        }

        private async Task FinishOldRequests()
        {
            while (true)
            {
                var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                foreach (var (guid, request) in UnfinishedRequests)
                {
                    var requestStartTime = request.StartTimeInMilliseconds;
                    var isRequestOld = now - requestStartTime > _maxRequestTimeInMilliseconds;

                    if (isRequestOld)
                        UnfinishedRequests.TryRemove(guid, out var dummy);
                }

                await Task.Delay(_frequencyOfFinishingOldRequests);
            }
        }

        public void SaveStartedRequest(string guid, string method, string url, long startTime)
        {
            var request = new UnfinishedRequest(guid, method, startTime);
            UnfinishedRequests.TryAdd(guid, request);
        }

        public void SaveFinishedRequest(string guid, long finish)
        {
            var isRemoved = UnfinishedRequests.TryRemove(guid, out var startedRequest);

            if (!isRemoved) return;

            var method = startedRequest.Method;
            var url = startedRequest.Url;
            var elapsedTime = (int) (finish - startedRequest.StartTimeInMilliseconds);
            var finishedRequest = new FinishedRequest(method, url, elapsedTime);
            FinishedRequests.Add(finishedRequest);
        }
    }
}