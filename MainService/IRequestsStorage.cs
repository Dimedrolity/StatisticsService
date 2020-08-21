using System.Collections.Concurrent;
using MainService.Requests;

namespace MainService
{
    public interface IRequestsStorage
    {
        ConcurrentDictionary<string, UnfinishedRequest> UnfinishedRequests { get; }
        ConcurrentDictionary<string, FinishedRequest> FinishedRequests { get; }
        ConcurrentBag<FailedRequest> FailedUdpRequests { get; }
        ConcurrentBag<FailedRequest> FailedHttpRequests { get; }

        void SaveStartedRequest(string guid, string method, string url, long startTime);
        void SaveFinishedRequest(string guid, long finishTime);
    }
}