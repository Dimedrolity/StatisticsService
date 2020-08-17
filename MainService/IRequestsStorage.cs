using System.Collections.Concurrent;
using MainService.Requests;

namespace MainService
{
    public interface IRequestsStorage
    {
        public ConcurrentDictionary<string, UnfinishedRequest> UnfinishedRequests { get; }
        public ConcurrentDictionary<string, FinishedRequest> FinishedRequests { get; }
        public ConcurrentBag<FailedRequest> FailedRequests { get; }

        public void SaveStartedRequest(string guid, string method, string url, long startTime);
        public void SaveFinishedRequest(string guid, long finishTime);
    }
}