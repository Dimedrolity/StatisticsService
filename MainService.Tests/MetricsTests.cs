using System.Collections.Generic;
using MainService.Metrics;
using MainService.Requests;
using NUnit.Framework;

namespace MainService.Tests
{
    public class MetricsTests
    {
        [Test]
        public void GetUnfinishedRequestsCount_IsCorrect()
        {
            var collector = new RequestsCollectorStub(
                new HashSet<UnfinishedRequest>
                {
                    new UnfinishedRequest("method", "url", 0),
                    new UnfinishedRequest("method", "url2", 0),
                },
                null);
            var metric = new UnfinishedRequestsCountMetric();

            var actual = metric.GetValue(collector);
            var expected = collector.UnfinishedRequests.Count.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetRequestsAverageTime_IsCorrect()
        {
            var collector = new RequestsCollectorStub(
                null,
                new HashSet<FinishedRequest>
                {
                    new FinishedRequest("method", "url", 100),
                    new FinishedRequest("method", "url2", 200),
                    new FinishedRequest("method", "url2", 300),
                });
            var metric = new RequestsAverageTimeMetric();

            var actual = metric.GetValue(collector);
            var expected = 200.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}