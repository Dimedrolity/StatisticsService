using System.Collections.Generic;
using MainService.Requests;

namespace MainService.Metrics
{
    public class RequestsWithErrorsCountMetric : Metric<FailedRequest>
    {
        public override string Name { get; } = "requestsWithErrorsCount";
        
        protected override string CalculateValue(ICollection<FailedRequest> requests)
        {
            return requests.Count.ToString();
        }
    }
}