using System.Linq;

namespace MainService.Metrics
{
    public class RequestsMinTimeMetric : Metric
    {
        public RequestsMinTimeMetric() : base("requestsMinTime")
        {
        }

        public override string GetValue(IRequestsCollector collector)
        {
            return (collector.FinishedRequests.Count == 0
                    ? 0
                    : collector.FinishedRequests
                        .Min(req => req.ElapsedTimeInMilliseconds))
                .ToString();
        }
    }
}