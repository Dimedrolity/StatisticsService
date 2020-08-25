using System.Linq;

namespace MainService.Metrics
{
    public class RequestsMaxTimeMetric : Metric
    {
        public override string Name { get; } = "requestsMaxTime";

        public override string GetValue(IRequestsStorage storage)
        {
            return (storage.FinishedRequests.Count == 0
                    ? 0
                    : storage.FinishedRequests.Values
                        .Max(req => req.ElapsedTimeInMilliseconds))
                .ToString();
        }
    }
}