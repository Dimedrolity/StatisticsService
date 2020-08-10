using System.Collections.Generic;
using System.Linq;
using MainService.Requests;
using NUnit.Framework;

namespace MainService.Tests
{
    public class MetricsTests
    {
        private IRequestsCollector _collector;
        private IMetrics _metrics;

        [Test]
        public void GetUnfinishedRequestsCount_IsCorrect()
        {
            _collector = new RequestsCollectorStub(
                new HashSet<UnfinishedRequest>
                {
                    new UnfinishedRequest("method", "url", 0),
                    new UnfinishedRequest("method", "url2", 0),
                },
                null);
            _metrics = new Metrics(_collector);

            var actual = _metrics.GetUnfinishedRequestsCount();
            var expected = _collector.UnfinishedRequests.Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetRequestsAverageTime_IsCorrect()
        {
            _collector = new RequestsCollectorStub(
                null,
                new HashSet<FinishedRequest>
                {
                    new FinishedRequest("method", "url", 100),
                    new FinishedRequest("method", "url2", 200),
                    new FinishedRequest("method", "url2", 300),
                });
            _metrics = new Metrics(_collector);

            var actual = (int) _metrics.GetRequestsAverageTime();
            var expected = (int) _collector.FinishedRequests
                .Average(r => r.ElapsedTimeInMilliseconds);

            Assert.AreEqual(expected, actual);
        }
    }
}