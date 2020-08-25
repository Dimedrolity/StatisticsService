namespace MainService.Metrics
{
    public class FinishedRequestsMetric : Metric
    {
        public override string Name { get; } = "finishedRequestsCount";

        public override string GetValue(IRequestsStorage storage)
        {
            return storage.FinishedRequests.Count.ToString();
        }
    }
}