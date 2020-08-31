using System.Collections.Concurrent;
using MainService.Requests;

namespace MainService
{
    public class RequestsStorage : IRequestsStorage
    {
        public ConcurrentDictionary<string, UnfinishedRequest> UnfinishedRequests { get; }
        public ConcurrentDictionary<string, FinishedRequest> FinishedRequests { get; }
        public ConcurrentBag<FailedRequest> LostUdpPackets { get; }
        public ConcurrentBag<FailedRequest> RequestsWithErrors { get; }

        public RequestsStorage()
        {
            UnfinishedRequests = new ConcurrentDictionary<string, UnfinishedRequest>();
            FinishedRequests = new ConcurrentDictionary<string, FinishedRequest>();
            LostUdpPackets = new ConcurrentBag<FailedRequest>();
            RequestsWithErrors = new ConcurrentBag<FailedRequest>();
        }

        public void SaveStartedRequest(string guid, string host, string method, long startTime)
        {
            var request = new UnfinishedRequest(host, method, startTime);
            UnfinishedRequests.TryAdd(guid, request);
        }

        public void SaveFinishedRequest(string guid, string host, string method, long finish)
        {
            var isRemoved = UnfinishedRequests.TryRemove(guid, out var startedRequest);

            if (isRemoved)
            {
                var elapsedTime = (int) (finish - startedRequest.StartTimeInMilliseconds);
                var finishedRequest = new FinishedRequest(host, method, elapsedTime);
                FinishedRequests.TryAdd(guid, finishedRequest);
            }
            else if (!UnfinishedRequests.ContainsKey(guid) && !FinishedRequests.ContainsKey(guid))
            {
                var unknownFailedRequest = new FailedRequest(host, method, finish);
                LostUdpPackets.Add(unknownFailedRequest);
            }
        }

        public void SaveRequestWithError(string guid, string host, string method, long failTime)
        {
            if (!UnfinishedRequests.TryRemove(guid, out _))
                return;

            var request = new FailedRequest(host, method, failTime);
            RequestsWithErrors.Add(request);
        }
    }
}