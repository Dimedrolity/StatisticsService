using System.Collections.Generic;
using MainService.Requests;

namespace MainService.Metrics
{
    public abstract class Metric<TRequest> where TRequest : Request
    {
        public abstract string Name { get; }

        public Dictionary<string, Dictionary<string, Dictionary<string, string>>>
            GetStatistics(Dictionary<string, Dictionary<string, List<TRequest>>> hostToMethodsToReqs)
        {
            if (hostToMethodsToReqs.Count == 0)
                return null;

            var statistics = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

            foreach (var (host, methodsToRequests) in hostToMethodsToReqs)
            {
                foreach (var (method, requests) in methodsToRequests)
                {
                    if (!statistics.ContainsKey(host))
                        statistics[host] = new Dictionary<string, Dictionary<string, string>>();

                    if (!statistics[host].ContainsKey(method))
                        statistics[host][method] = new Dictionary<string, string>();

                    statistics[host][method].Add(Name, CalculateValue(requests));
                }
            }

            return statistics;
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> GetStatistics(
            Dictionary<string, Dictionary<string, List<TRequest>>> hostToMethodsToReqs, string host)
        {
            if (hostToMethodsToReqs.Count == 0)
                return null;

            var statistics = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

            var methodsToRequests = hostToMethodsToReqs[host];

            foreach (var (method, requests) in methodsToRequests)
            {
                if (!statistics.ContainsKey(host))
                    statistics[host] = new Dictionary<string, Dictionary<string, string>>();

                if (!statistics[host].ContainsKey(method))
                    statistics[host][method] = new Dictionary<string, string>();

                statistics[host][method].Add(Name, CalculateValue(requests));
            }

            return statistics;
        }

        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> GetStatistics(
            Dictionary<string, Dictionary<string, List<TRequest>>> hostToMethodsToReqs, string host, string method)
        {
            if (hostToMethodsToReqs.Count == 0)
                return null;

            var statistics = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

            var methodsToRequests = hostToMethodsToReqs[host];
            var requests = methodsToRequests[method];

            if (!statistics.ContainsKey(host))
                statistics[host] = new Dictionary<string, Dictionary<string, string>>();

            if (!statistics[host].ContainsKey(method))
                statistics[host][method] = new Dictionary<string, string>();

            statistics[host][method].Add(Name, CalculateValue(requests));

            return statistics;
        }

        protected abstract string CalculateValue(ICollection<TRequest> requests);
    }
}