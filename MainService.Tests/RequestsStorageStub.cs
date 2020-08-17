using System.Collections.Concurrent;
using MainService.Requests;

namespace MainService.Tests
{
    public class RequestsStorageStub : IRequestsStorage
    {
        public ConcurrentDictionary<string, UnfinishedRequest> UnfinishedRequests { get; }
        public ConcurrentDictionary<string, FinishedRequest> FinishedRequests { get; }
        public ConcurrentBag<FailedRequest> FailedRequests { get; }

        public RequestsStorageStub(ConcurrentDictionary<string, UnfinishedRequest> unfinishedRequests,
            ConcurrentDictionary<string, FinishedRequest> finishedRequests, ConcurrentBag<FailedRequest> failedRequests)
        {
            UnfinishedRequests = unfinishedRequests;
            FinishedRequests = finishedRequests;
            FailedRequests = failedRequests;
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