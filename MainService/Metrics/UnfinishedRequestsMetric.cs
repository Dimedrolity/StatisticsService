namespace MainService.Metrics
{
    public class UnfinishedRequestsMetric : Metric
    {
        public UnfinishedRequestsMetric() : base("unfinishedRequestsCount")
        {
        }

        public override string GetValue(IRequestsStorage storage)
        {
            return storage.UnfinishedRequests.Count.ToString();
        }
    }
}