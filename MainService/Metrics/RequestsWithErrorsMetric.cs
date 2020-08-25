namespace MainService.Metrics
{
    public class RequestsWithErrorsMetric : Metric
    {
        public override string Name { get; } = "requestsWithErrorsCount";

        public override string GetValue(IRequestsStorage storage)
        {
            return storage.RequestsWithErrors.Count.ToString();
        }
    }
}