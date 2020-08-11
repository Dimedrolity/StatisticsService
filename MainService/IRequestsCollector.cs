using System.Collections.Concurrent;
using MainService.Requests;

namespace MainService
{
    public interface IRequestsCollector
    {
        public ConcurrentDictionary<string, UnfinishedRequest> UnfinishedRequests { get; }
        public ConcurrentBag<FinishedRequest> FinishedRequests { get; }

        public void SaveStartedRequest(string guid, string method, string url, long startTime);
        public void SaveFinishedRequest(string guid, long finishTime);
    }
}