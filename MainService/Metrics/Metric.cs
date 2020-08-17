namespace MainService.Metrics
{
    public abstract class Metric
    {
        public string Name { get; }

        public abstract string GetValue(IRequestsStorage storage);

        protected Metric(string name)
        {
            Name = name;
        }
    }
}