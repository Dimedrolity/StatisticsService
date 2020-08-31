using System.Collections.Generic;
using MainService.Requests;

namespace MainService
{
    public class RequestsProvider : IRequestsProvider
    {
        private readonly IRequestsStorage _storage;

        public RequestsProvider(IRequestsStorage storage)
        {
            _storage = storage;
        }

        public Dictionary<string, Dictionary<string, List<UnfinishedRequest>>>
            GetUnfinishedRequestsInHierarchicalStructure()
        {
            var unfinishedRequests = _storage.UnfinishedRequests.Values;

            return ConvertRequestsToHierarchicalStructure(unfinishedRequests);
        }

        public Dictionary<string, Dictionary<string, List<FinishedRequest>>>
            GetFinishedRequestsInHierarchicalStructure()
        {
            var finishedRequests = _storage.FinishedRequests.Values;

            return ConvertRequestsToHierarchicalStructure(finishedRequests);
        }

        public Dictionary<string, Dictionary<string, List<FailedRequest>>>
            GetRequestsWithErrorsInHierarchicalStructure()
        {
            var requestsWithErrors = _storage.RequestsWithErrors;

            return ConvertRequestsToHierarchicalStructure(requestsWithErrors);
        }

        public Dictionary<string, Dictionary<string, List<FailedRequest>>> GetLostUdpPacketsInHierarchicalStructure()
        {
            var lostUdpPackets = _storage.LostUdpPackets;

            return ConvertRequestsToHierarchicalStructure(lostUdpPackets);
        }

        private static Dictionary<string, Dictionary<string, List<TRequest>>>
            ConvertRequestsToHierarchicalStructure<TRequest>(IEnumerable<TRequest> requests) where TRequest : Request
        {
            var hostToMethodsToRequests = new Dictionary<string, Dictionary<string, List<TRequest>>>();

            foreach (var request in requests)
            {
                if (!hostToMethodsToRequests.ContainsKey(request.Host))
                    hostToMethodsToRequests[request.Host] = new Dictionary<string, List<TRequest>>();

                if (!hostToMethodsToRequests[request.Host].ContainsKey(request.Method))
                    hostToMethodsToRequests[request.Host][request.Method] = new List<TRequest>();

                hostToMethodsToRequests[request.Host][request.Method].Add(request);
            }

            return hostToMethodsToRequests;
        }
    }
}