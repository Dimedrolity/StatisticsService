using System.Collections.Generic;
using System.Linq;
using MainService.Requests;

namespace MainService.Metrics
{
    public class RequestsAverageTimeMetric : Metric<FinishedRequest>
    {
        public override string Name { get; } = "requestsAverageTime";

        protected override string CalculateValue(ICollection<FinishedRequest> requests)
        {
            return (requests.Count == 0
                    ? 0
                    : (int) requests
                        .Average(req => req.ElapsedTimeInMilliseconds))
                .ToString();
        }
    }
}