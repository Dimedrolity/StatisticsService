using System.Collections.Concurrent;
using MainService.Metrics;
using MainService.Requests;
using NUnit.Framework;

namespace MainService.Tests
{
    public class MetricsTests
    {
        [Test]
        public void UnfinishedRequestsCountMetric_IsCorrect()
        {
            var dictionary = new ConcurrentDictionary<string, UnfinishedRequest>();
            dictionary.TryAdd("123", new UnfinishedRequest("method", "url", 0));
            dictionary.TryAdd("456", new UnfinishedRequest("method", "url2", 0));
            var collector = new RequestsCollectorStub(dictionary, null);
            var metric = new UnfinishedRequestsCountMetric();

            var actual = metric.GetValue(collector);
            var expected = collector.UnfinishedRequests.Count.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsAverageTimeMetric_IsCorrect()
        {
            var collector = new RequestsCollectorStub(
                null,
                new ConcurrentBag<FinishedRequest>
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

        [Test]
        public void RequestsMinTimeMetric_IsCorrect()
        {
            var collector = new RequestsCollectorStub(
                null,
                new ConcurrentBag<FinishedRequest>
                {
                    new FinishedRequest("method", "url", 100),
                    new FinishedRequest("method", "url2", 200),
                    new FinishedRequest("method", "url2", 300),
                });
            var metric = new RequestsMinTimeMetric();

            var actual = metric.GetValue(collector);
            var expected = 100.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsMaxTimeMetric_IsCorrect()
        {
            var collector = new RequestsCollectorStub(
                null,
                new ConcurrentBag<FinishedRequest>
                {
                    new FinishedRequest("method", "url", 100),
                    new FinishedRequest("method", "url2", 200),
                    new FinishedRequest("method", "url2", 300),
                });
            var metric = new RequestsMaxTimeMetric();

            var actual = metric.GetValue(collector);
            var expected = 300.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsMedianTimeMetric_EvenRequestsCount_IsCorrect()
        {
            var collector = new RequestsCollectorStub(
                null,
                new ConcurrentBag<FinishedRequest>
                {
                    new FinishedRequest("method", "url", 100),
                    new FinishedRequest("method", "url2", 200),
                    new FinishedRequest("method", "url2", 400),
                    new FinishedRequest("method", "url2", 800),
                });
            var metric = new RequestsMedianTimeMetric();

            var actual = metric.GetValue(collector);
            var expected = 300.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsMedianTimeMetric_OddRequestsCount_IsCorrect()
        {
            var collector = new RequestsCollectorStub(
                null,
                new ConcurrentBag<FinishedRequest>
                {
                    new FinishedRequest("method", "url2", 333),
                    new FinishedRequest("method", "url2", 444),
                    new FinishedRequest("method", "url2", 555),
                });
            var metric = new RequestsMedianTimeMetric();

            var actual = metric.GetValue(collector);
            var expected = 444.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}