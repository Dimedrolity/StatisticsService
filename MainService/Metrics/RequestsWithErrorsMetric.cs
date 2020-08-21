namespace MainService.Metrics
{
    public class RequestsWithErrorsMetric : Metric
    {
        public RequestsWithErrorsMetric() : base("requestsWithErrorsCount")
        {
        }

        public override string GetValue(IRequestsStorage storage)
        {
            return storage.RequestsWithErrors.Count.ToString();
        }
    }
}