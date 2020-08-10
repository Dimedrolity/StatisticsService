using System.Collections.Generic;
using MainService.Metrics;

namespace MainService
{
    public class MetricsProvider : IMetricsProvider
    {
        private readonly IEnumerable<Metric> _metrics;

        public MetricsProvider(IEnumerable<Metric> metrics)
        {
            _metrics = metrics;
        }

        public IEnumerable<Metric> GetAllMetrics()
        {
            return _metrics;
        }
    }
}