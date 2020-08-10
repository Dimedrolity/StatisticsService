using System.Linq;

namespace MainService
{
    public class Metrics : IMetrics
    {
        private readonly IRequestsCollector _collector;

        public Metrics(IRequestsCollector collector)
        {
            _collector = collector;
        }

        public int GetUnfinishedRequestsCount()
        {
            return _collector.UnfinishedRequests.Count;
        }

        public double GetRequestsAverageTime()
        {
            return _collector.FinishedRequests.Count == 0
                ? 0
                : _collector.FinishedRequests.Average(req => req.ElapsedTimeInMilliseconds);
        }
    }
}