using System.Linq;

namespace MainService.Metrics
{
    public class RequestsMinTimeMetric : Metric
    {
        public RequestsMinTimeMetric() : base("requestsMinTime")
        {
        }

        public override string GetValue(IRequestsStorage storage)
        {
            return (storage.FinishedRequests.Count == 0
                    ? 0
                    : storage.FinishedRequests.Values
                        .Min(req => req.ElapsedTimeInMilliseconds))
                .ToString();
        }
    }
}