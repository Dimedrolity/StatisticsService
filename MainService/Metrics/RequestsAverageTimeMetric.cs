using System.Linq;

namespace MainService.Metrics
{
    public class RequestsAverageTimeMetric : Metric
    {
        public RequestsAverageTimeMetric() : base("requestsAverageTime")
        {
        }

        public override string GetValue(IRequestsStorage storage)
        {
            return (storage.FinishedRequests.Count == 0
                    ? 0
                    : (int) storage.FinishedRequests.Values
                        .Average(req => req.ElapsedTimeInMilliseconds))
                .ToString();
        }
    }
}