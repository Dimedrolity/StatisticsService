using System.Collections.Generic;
using MainService.Requests;

namespace MainService.Metrics
{
    public class UnfinishedRequestsCountMetric : Metric<UnfinishedRequest>
    {
        public override string Name { get; } = "unfinishedRequestsCount";

        protected override string CalculateValue(ICollection<UnfinishedRequest> requests)
        {
            return requests.Count.ToString();
        }
    }
}