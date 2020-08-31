using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MainService
{
    public class StatisticsProvider : IStatisticsProvider
    {
        private readonly IRequestsProvider _requestsProvider;
        private readonly IMetricsProvider _metricsProvider;

        public StatisticsProvider(IRequestsProvider requestsProvider, IMetricsProvider metricsProvider)
        {
            _requestsProvider = requestsProvider;
            _metricsProvider = metricsProvider;
        }

        public string GetStatistics()
        {
            var unfinishedRequests = _requestsProvider.GetUnfinishedRequestsInHierarchicalStructure();
            var statisticsForUnfinishedRequests =
                _metricsProvider.GetMetricsForUnfinishedRequests()
                    .Select(metric => metric.GetStatistics(unfinishedRequests))
                    .Where(a => a != null);

            var finishedRequests = _requestsProvider.GetFinishedRequestsInHierarchicalStructure();
            var statisticsForFinishedRequests =
                _metricsProvider.GetMetricsForFinishedRequests()
                    .Select(metric => metric.GetStatistics(finishedRequests))
                    .Where(a => a != null);

            var requestsWithErrors = _requestsProvider.GetRequestsWithErrorsInHierarchicalStructure();
            var statisticsForRequestsWithErrors =
                _metricsProvider.GetMetricsForRequestsWithErrors()
                    .Select(metric => metric.GetStatistics(requestsWithErrors))
                    .Where(a => a != null);

            var lostUdpPackets = _requestsProvider.GetLostUdpPacketsInHierarchicalStructure();
            var statisticsForLostUdpPackets =
                _metricsProvider.GetMetricsForLostUdpPackets()
                    .Select(metric => metric.GetStatistics(lostUdpPackets))
                    .Where(a => a != null);

            var mergedStatistics = MergeStatistics(statisticsForUnfinishedRequests,
                statisticsForFinishedRequests, statisticsForRequestsWithErrors, statisticsForLostUdpPackets);

            return JsonConvert.SerializeObject(mergedStatistics);
        }


        public string GetStatistics(string host)
        {
            var unfinishedRequests = _requestsProvider.GetUnfinishedRequestsInHierarchicalStructure();
            var statisticsForUnfinishedRequests =
                _metricsProvider.GetMetricsForUnfinishedRequests()
                    .Select(metric => metric.GetStatistics(unfinishedRequests, host))
                    .Where(a => a != null);

            var finishedRequests = _requestsProvider.GetFinishedRequestsInHierarchicalStructure();
            var statisticsForFinishedRequests =
                _metricsProvider.GetMetricsForFinishedRequests()
                    .Select(metric => metric.GetStatistics(finishedRequests, host))
                    .Where(a => a != null);

            var requestsWithErrors = _requestsProvider.GetRequestsWithErrorsInHierarchicalStructure();
            var statisticsForRequestsWithErrors =
                _metricsProvider.GetMetricsForRequestsWithErrors()
                    .Select(metric => metric.GetStatistics(requestsWithErrors, host))
                    .Where(a => a != null);

            var lostUdpPackets = _requestsProvider.GetLostUdpPacketsInHierarchicalStructure();
            var statisticsForLostUdpPackets =
                _metricsProvider.GetMetricsForLostUdpPackets()
                    .Select(metric => metric.GetStatistics(lostUdpPackets, host))
                    .Where(a => a != null);

            var mergedStatistics = MergeStatistics(statisticsForUnfinishedRequests,
                statisticsForFinishedRequests, statisticsForRequestsWithErrors, statisticsForLostUdpPackets);

            return JsonConvert.SerializeObject(mergedStatistics);
        }

        public string GetStatistics(string host, string method)
        {
            var unfinishedRequests = _requestsProvider.GetUnfinishedRequestsInHierarchicalStructure();
            var statisticsForUnfinishedRequests =
                _metricsProvider.GetMetricsForUnfinishedRequests()
                    .Select(metric => metric.GetStatistics(unfinishedRequests, host, method))
                    .Where(a => a != null);

            var finishedRequests = _requestsProvider.GetFinishedRequestsInHierarchicalStructure();
            var statisticsForFinishedRequests =
                _metricsProvider.GetMetricsForFinishedRequests()
                    .Select(metric => metric.GetStatistics(finishedRequests, host, method))
                    .Where(a => a != null);

            var requestsWithErrors = _requestsProvider.GetRequestsWithErrorsInHierarchicalStructure();
            var statisticsForRequestsWithErrors =
                _metricsProvider.GetMetricsForRequestsWithErrors()
                    .Select(metric => metric.GetStatistics(requestsWithErrors, host, method))
                    .Where(a => a != null);

            var lostUdpPackets = _requestsProvider.GetLostUdpPacketsInHierarchicalStructure();
            var statisticsForLostUdpPackets =
                _metricsProvider.GetMetricsForLostUdpPackets()
                    .Select(metric => metric.GetStatistics(lostUdpPackets, host, method))
                    .Where(a => a != null);

            var mergedStatistics = MergeStatistics(statisticsForUnfinishedRequests,
                statisticsForFinishedRequests, statisticsForRequestsWithErrors, statisticsForLostUdpPackets);

            return JsonConvert.SerializeObject(mergedStatistics);
        }

        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> MergeStatistics(
            IEnumerable<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>
                statisticsForUnfinishedRequests,
            IEnumerable<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>
                statisticsForFinishedRequests,
            IEnumerable<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>
                statisticsForRequestsWithErrors,
            IEnumerable<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>
                statisticsForLostUdpPackets)
        {
            var allStatistics = statisticsForUnfinishedRequests
                .Concat(statisticsForFinishedRequests)
                .Concat(statisticsForRequestsWithErrors)
                .Concat(statisticsForLostUdpPackets);

            return allStatistics.Merge();
        }
    }

    public static class DictionaryExtensions
    {
        public static Dictionary<T1, Dictionary<T2, Dictionary<T3, T4>>>
            Merge<T1, T2, T3, T4>(this IEnumerable<Dictionary<T1, Dictionary<T2, Dictionary<T3, T4>>>> dictionaries)
        {
            var result = new Dictionary<T1, Dictionary<T2, Dictionary<T3, T4>>>();

            foreach (var dictionary in dictionaries)
            foreach (var (firstKey, firstValue) in dictionary)
            {
                if (!result.ContainsKey(firstKey))
                {
                    result.Add(firstKey, firstValue);
                }
                else
                {
                    foreach (var (secondKey, secondValue) in firstValue)
                    {
                        if (!result[firstKey].ContainsKey(secondKey))
                        {
                            result[firstKey].Add(secondKey, secondValue);
                        }
                        else
                        {
                            foreach (var (thirdKey, thirdValue) in secondValue)
                            {
                                result[firstKey][secondKey].Add(thirdKey, thirdValue);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}