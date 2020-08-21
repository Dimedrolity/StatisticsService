using System.Collections.Concurrent;
using MainService.Requests;

namespace MainService
{
    public class RequestsStorage : IRequestsStorage
    {
        public ConcurrentDictionary<string, UnfinishedRequest> UnfinishedRequests { get; }
        public ConcurrentDictionary<string, FinishedRequest> FinishedRequests { get; }
        public ConcurrentBag<FailedRequest> FailedUdpRequests { get; }
        public ConcurrentBag<FailedRequest> FailedHttpRequests { get; }

        public RequestsStorage()
        {
            UnfinishedRequests = new ConcurrentDictionary<string, UnfinishedRequest>();
            FinishedRequests = new ConcurrentDictionary<string, FinishedRequest>();
            FailedUdpRequests = new ConcurrentBag<FailedRequest>();
            FailedHttpRequests = new ConcurrentBag<FailedRequest>();
        }

        public void SaveStartedRequest(string guid, string method, string url, long startTime)
        {
            var request = new UnfinishedRequest(guid, method, startTime);
            UnfinishedRequests.TryAdd(guid, request);
        }

        public void SaveFinishedRequest(string guid, long finish)
        {
            var isRemoved = UnfinishedRequests.TryRemove(guid, out var startedRequest);

            if (isRemoved)
            {
                var method = startedRequest.Method;
                var url = startedRequest.Url;
                var elapsedTime = (int) (finish - startedRequest.StartTimeInMilliseconds);
                var finishedRequest = new FinishedRequest(method, url, elapsedTime);
                FinishedRequests.TryAdd(guid, finishedRequest);
            }
            else if (!UnfinishedRequests.ContainsKey(guid) && !FinishedRequests.ContainsKey(guid))
            {
                var unknownFailedRequest = new FailedRequest("no method", "no url", finish);
                FailedUdpRequests.Add(unknownFailedRequest);
            }
        }
    }
}