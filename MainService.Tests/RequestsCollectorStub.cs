using System.Collections.Generic;
using MainService.Requests;

namespace MainService.Tests
{
    public class RequestsCollectorStub : IRequestsCollector
    {
        public HashSet<UnfinishedRequest> UnfinishedRequests { get; }
        public HashSet<FinishedRequest> FinishedRequests { get; }

        public RequestsCollectorStub(HashSet<UnfinishedRequest> unfinishedRequests, HashSet<FinishedRequest> finishedRequests)
        {
            UnfinishedRequests = unfinishedRequests;
            FinishedRequests = finishedRequests;
        }
        
        public void SaveStartedRequest(string method, string url, long startTime)
        {
            throw new System.NotImplementedException();
        }

        public void SaveFinishedRequest(string method, string url, long finish)
        {
            throw new System.NotImplementedException();
        }
    }
}