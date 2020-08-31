using System.Collections.Generic;
using MainService.Requests;

namespace MainService
{
    public interface IRequestsProvider
    {
        Dictionary<string, Dictionary<string, List<UnfinishedRequest>>> GetUnfinishedRequestsInHierarchicalStructure();
        Dictionary<string, Dictionary<string, List<FinishedRequest>>> GetFinishedRequestsInHierarchicalStructure();
        Dictionary<string, Dictionary<string, List<FailedRequest>>> GetRequestsWithErrorsInHierarchicalStructure();
        Dictionary<string, Dictionary<string, List<FailedRequest>>> GetLostUdpPacketsInHierarchicalStructure();
    }
}