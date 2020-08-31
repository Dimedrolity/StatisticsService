using System.Collections.Concurrent;
using MainService.Requests;

namespace MainService
{
    public interface IRequestsStorage
    {
        ConcurrentDictionary<string, UnfinishedRequest> UnfinishedRequests { get; }
        ConcurrentDictionary<string, FinishedRequest> FinishedRequests { get; }
        ConcurrentBag<FailedRequest> LostUdpPackets { get; }
        ConcurrentBag<FailedRequest> RequestsWithErrors { get; }

        void SaveStartedRequest(string guid, string host, string method, long startTime);
        void SaveFinishedRequest(string guid, string host, string method, long finishTime);
        void SaveRequestWithError(string guid, string host, string method, long failTime);
    }
}