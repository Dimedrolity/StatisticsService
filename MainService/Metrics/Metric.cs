namespace MainService.Metrics
{
    public abstract class Metric
    {
        public abstract string Name { get; }
        public abstract string GetValue(IRequestsStorage storage);
    }
}