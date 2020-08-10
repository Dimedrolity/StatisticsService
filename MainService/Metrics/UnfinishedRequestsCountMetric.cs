namespace MainService.Metrics
{
    public class UnfinishedRequestsCountMetric : Metric
    {
        public UnfinishedRequestsCountMetric() : base("unfinishedRequestsCount")
        {
        }

        public override string GetValue(IRequestsCollector collector)
        {
            return collector.UnfinishedRequests.Count.ToString();
        }
    }
}