namespace MainService.Metrics
{
    public class UnfinishedRequestsMetric : Metric
    {
        public override string Name { get; } = "unfinishedRequestsCount";

        public override string GetValue(IRequestsStorage storage)
        {
            return storage.UnfinishedRequests.Count.ToString();
        }
    }
}