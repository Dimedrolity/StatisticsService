using System.Collections.Generic;
using MainService.Requests;

namespace MainService.Metrics
{
    public class FinishedRequestsCountMetric : Metric<FinishedRequest>
    {
        public override string Name { get; } = "finishedRequestsCount";

        protected override string CalculateValue(ICollection<FinishedRequest> requests)
        {
            return requests.Count.ToString();
        }
    }
}