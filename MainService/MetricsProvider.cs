using System.Collections.Generic;
using MainService.Metrics;
using MainService.Requests;

namespace MainService
{
    public class MetricsProvider : IMetricsProvider
    {
        private readonly IEnumerable<Metric<FinishedRequest>> _metricsForFinishedRequests;
        private readonly IEnumerable<Metric<UnfinishedRequest>> _metricsForUnfinishedRequests;
        private readonly IEnumerable<Metric<FailedRequest>> _metricsForRequestsWithErrors;
        private readonly IEnumerable<Metric<FailedRequest>> _metricsForLostUdpPackets;

        public MetricsProvider(IEnumerable<Metric<FinishedRequest>> metricsForFinishedRequests,
            IEnumerable<Metric<UnfinishedRequest>> metricsForUnfinishedRequests,
            IEnumerable<Metric<FailedRequest>> metricsForRequestsWithErrors,
            IEnumerable<Metric<FailedRequest>> metricsForLostUdpPackets)
        {
            _metricsForFinishedRequests = metricsForFinishedRequests;
            _metricsForUnfinishedRequests = metricsForUnfinishedRequests;
            _metricsForRequestsWithErrors = metricsForRequestsWithErrors;
            _metricsForLostUdpPackets = metricsForLostUdpPackets;
        }

        public IEnumerable<Metric<FinishedRequest>> GetMetricsForFinishedRequests()
        {
            return _metricsForFinishedRequests;
        }

        public IEnumerable<Metric<UnfinishedRequest>> GetMetricsForUnfinishedRequests()
        {
            return _metricsForUnfinishedRequests;
        }

        public IEnumerable<Metric<FailedRequest>> GetMetricsForRequestsWithErrors()
        {
            return _metricsForRequestsWithErrors;
        }

        public IEnumerable<Metric<FailedRequest>> GetMetricsForLostUdpPackets()
        {
            return _metricsForLostUdpPackets;
        }
    }
}