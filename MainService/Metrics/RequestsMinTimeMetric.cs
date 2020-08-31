using System.Collections.Generic;
using System.Linq;
using MainService.Requests;

namespace MainService.Metrics
{
    public class RequestsMinTimeMetric : Metric<FinishedRequest>
    {
        public override string Name { get; } = "requestsMinTime";

        protected override string CalculateValue(ICollection<FinishedRequest> requests)
        {
            return (requests.Count == 0
                    ? 0
                    : requests
                        .Min(req => req.ElapsedTimeInMilliseconds))
                .ToString();
        }
    }
}