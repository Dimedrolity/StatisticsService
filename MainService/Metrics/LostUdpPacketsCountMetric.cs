using System.Collections.Generic;
using MainService.Requests;

namespace MainService.Metrics
{
    public class LostUdpPacketsCountMetric : Metric<FailedRequest>
    {
        public override string Name { get; } = "lostUdpPacketsCount";

        protected override string CalculateValue(ICollection<FailedRequest> requests)
        {
            return requests.Count.ToString();
        }
    }
}