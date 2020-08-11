using System.Collections.Concurrent;
using MainService.Requests;

namespace MainService.Tests
{
    public class RequestsCollectorStub : IRequestsCollector
    {
        public ConcurrentDictionary<string, UnfinishedRequest> UnfinishedRequests { get; }
        public ConcurrentBag<FinishedRequest> FinishedRequests { get; }
        
        public RequestsCollectorStub(ConcurrentDictionary<string, UnfinishedRequest> unfinishedRequests,
            ConcurrentBag<FinishedRequest> finishedRequests)
        {
            UnfinishedRequests = unfinishedRequests;
            FinishedRequests = finishedRequests;
        }

        public void SaveStartedRequest(string guid, string method, string url, long startTime)
        {
            throw new System.NotImplementedException();
        }

        public void SaveFinishedRequest(string guid, long finishTime)
        {
            throw new System.NotImplementedException();
        }
    }
}