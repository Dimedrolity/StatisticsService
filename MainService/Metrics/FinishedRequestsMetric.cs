namespace MainService.Metrics
{
    public class FinishedRequestsMetric : Metric
    {
        public FinishedRequestsMetric() : base("finishedRequestsCount")
        {
        }

        public override string GetValue(IRequestsStorage storage)
        {
            return storage.FinishedRequests.Count.ToString();
        }
    }
}