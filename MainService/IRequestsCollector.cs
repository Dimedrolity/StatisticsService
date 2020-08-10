using System.Collections.Generic;
using MainService.Requests;

namespace MainService
{
    public interface IRequestsCollector
    {
        public HashSet<UnfinishedRequest> UnfinishedRequests { get; }
        public HashSet<FinishedRequest> FinishedRequests { get; }

        public void SaveStartedRequest(string method, string url, long startTime);
        public void SaveFinishedRequest(string method, string url, long finish);
    }
}