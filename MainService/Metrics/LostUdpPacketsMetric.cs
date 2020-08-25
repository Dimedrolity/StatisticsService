namespace MainService.Metrics
{
    public class LostUdpPacketsMetric : Metric
    {
        public override string Name { get; } = "lostUdpPacketsCount";

        public override string GetValue(IRequestsStorage storage)
        {
            return storage.LostUdpPackets.Count.ToString();
        }
    }
}