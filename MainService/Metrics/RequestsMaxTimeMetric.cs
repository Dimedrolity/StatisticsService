using System.Collections.Generic;
using System.Linq;
using MainService.Requests;

namespace MainService.Metrics
{
    public class RequestsMaxTimeMetric : Metric<FinishedRequest>
    {
        public override string Name { get; } = "requestsMaxTime";

        protected override string CalculateValue(ICollection<FinishedRequest> requests)
        {
            return (requests.Count == 0
                    ? 0
                    : requests
                        .Max(req => req.ElapsedTimeInMilliseconds))
                .ToString();
        }
    }
}