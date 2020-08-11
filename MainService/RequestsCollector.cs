using System;
using System.Collections.Concurrent;
using MainService.Requests;

namespace MainService
{
    public class RequestsCollector : IRequestsCollector
    {
        public ConcurrentDictionary<string, UnfinishedRequest> UnfinishedRequests { get; }
        public ConcurrentBag<FinishedRequest> FinishedRequests { get; }

        public RequestsCollector()
        {
            UnfinishedRequests = new ConcurrentDictionary<string, UnfinishedRequest>();
            FinishedRequests = new ConcurrentBag<FinishedRequest>();
        }

        public void SaveStartedRequest(string guid, string method, string url, long startTime)
        {
            var request = new UnfinishedRequest(guid, method, startTime);
            UnfinishedRequests.TryAdd(guid, request);
        }

        public void SaveFinishedRequest(string guid, long finish)
        {
            var isRemoved = UnfinishedRequests.TryRemove(guid, out var startedRequest);
            if (!isRemoved)
                throw new ArgumentException(
                    $"Не удалось удалить элемент из {nameof(UnfinishedRequests)} с guid = {guid}");

            var method = startedRequest.Method;
            var url = startedRequest.Url;
            var elapsedTime = (int) (finish - startedRequest.StartTimeInMilliseconds);
            var finishedRequest = new FinishedRequest(method, url, elapsedTime);
            FinishedRequests.Add(finishedRequest);
        }
    }
}