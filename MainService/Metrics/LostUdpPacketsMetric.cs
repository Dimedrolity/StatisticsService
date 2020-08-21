namespace MainService.Metrics
{
    public class LostUdpPacketsMetric : Metric
    {
        public LostUdpPacketsMetric() : base("lostUdpPacketsCount")
        {
        }

        public override string GetValue(IRequestsStorage storage)
        {
            return storage.LostUdpPackets.Count.ToString();
        }
    }
}