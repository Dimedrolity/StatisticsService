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
}