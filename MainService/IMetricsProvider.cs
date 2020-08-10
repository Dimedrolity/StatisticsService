using System.Collections.Generic;
using MainService.Metrics;

namespace MainService
{
    public interface IMetricsProvider
    {
        public IEnumerable<Metric> GetAllMetrics();
    }
}