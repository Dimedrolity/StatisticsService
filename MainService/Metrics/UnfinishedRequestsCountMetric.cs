namespace MainService.Metrics
{
    public class UnfinishedRequestsCountMetric : Metric
    {
        public UnfinishedRequestsCountMetric() : base("unfinishedRequestsCount")
        {
        }

        public override string GetValue(IRequestsStorage storage)
        {
            return storage.UnfinishedRequests.Count.ToString();
        }
    }
}