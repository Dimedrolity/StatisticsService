using System.Collections.Generic;
using MainService.Metrics;
using MainService.Requests;

namespace MainService
{
    public interface IMetricsProvider
    {
        IEnumerable<Metric<FinishedRequest>> GetMetricsForFinishedRequests();
        IEnumerable<Metric<UnfinishedRequest>> GetMetricsForUnfinishedRequests();
        IEnumerable<Metric<FailedRequest>> GetMetricsForRequestsWithErrors();
        IEnumerable<Metric<FailedRequest>> GetMetricsForLostUdpPackets();
    }
}