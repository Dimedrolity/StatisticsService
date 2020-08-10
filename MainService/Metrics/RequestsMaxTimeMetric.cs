using System.Linq;

namespace MainService.Metrics
{
   public class RequestsMaxTimeMetric : Metric
    {
        public RequestsMaxTimeMetric() : base("requestsMaxTime")
        {
        }

        public override string GetValue(IRequestsCollector collector)
        {
            return (collector.FinishedRequests.Count == 0
                    ? 0
                    : (int) collector.FinishedRequests
                        .Max(req => req.ElapsedTimeInMilliseconds))
                .ToString();
        }
    }
}