using System.Linq;

namespace MainService.Metrics
{
    public class RequestsAverageTimeMetric : Metric
    {
        public RequestsAverageTimeMetric() : base("requestsAverageTime")
        {
        }

        public override string GetValue(IRequestsCollector collector)
        {
            return (collector.FinishedRequests.Count == 0
                    ? 0
                    : (int) collector.FinishedRequests.Values
                        .Average(req => req.ElapsedTimeInMilliseconds))
                .ToString();
        }
    }
}