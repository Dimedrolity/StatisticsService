using System.Collections.Concurrent;
using FakeItEasy;
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
            var unfinishedRequests = new ConcurrentDictionary<string, UnfinishedRequest>();
            unfinishedRequests.TryAdd("123", new UnfinishedRequest("method", "url", 0));
            unfinishedRequests.TryAdd("456", new UnfinishedRequest("method", "url2", 0));

            var storage = A.Fake<IRequestsStorage>();
            A.CallTo(() => storage.UnfinishedRequests).Returns(unfinishedRequests);

            var metric = new UnfinishedRequestsCountMetric();

            var actual = metric.GetValue(storage);
            var expected = storage.UnfinishedRequests.Count.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsAverageTimeMetric_IsCorrect()
        {
            var finishedRequests = new ConcurrentDictionary<string, FinishedRequest>();
            finishedRequests.TryAdd("1", new FinishedRequest("method", "url", 100));
            finishedRequests.TryAdd("2", new FinishedRequest("method", "url2", 200));
            finishedRequests.TryAdd("3", new FinishedRequest("method", "url2", 300));

            var storage = A.Fake<IRequestsStorage>();
            A.CallTo(() => storage.FinishedRequests).Returns(finishedRequests);

            var metric = new RequestsAverageTimeMetric();

            var actual = metric.GetValue(storage);
            var expected = 200.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsMinTimeMetric_IsCorrect()
        {
            var finishedRequests = new ConcurrentDictionary<string, FinishedRequest>();
            finishedRequests.TryAdd("1", new FinishedRequest("method", "url", 100));
            finishedRequests.TryAdd("2", new FinishedRequest("method", "url2", 200));
            finishedRequests.TryAdd("3", new FinishedRequest("method", "url2", 300));

            var storage = A.Fake<IRequestsStorage>();
            A.CallTo(() => storage.FinishedRequests).Returns(finishedRequests);

            var metric = new RequestsMinTimeMetric();

            var actual = metric.GetValue(storage);
            var expected = 100.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsMaxTimeMetric_IsCorrect()
        {
            var finishedRequests = new ConcurrentDictionary<string, FinishedRequest>();
            finishedRequests.TryAdd("1", new FinishedRequest("method", "url", 100));
            finishedRequests.TryAdd("2", new FinishedRequest("method", "url2", 200));
            finishedRequests.TryAdd("3", new FinishedRequest("method", "url2", 300));

            var storage = A.Fake<IRequestsStorage>();
            A.CallTo(() => storage.FinishedRequests).Returns(finishedRequests);

            var metric = new RequestsMaxTimeMetric();

            var actual = metric.GetValue(storage);
            var expected = 300.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsMedianTimeMetric_EvenRequestsCount_IsCorrect()
        {
            var finishedRequests = new ConcurrentDictionary<string, FinishedRequest>();
            finishedRequests.TryAdd("1", new FinishedRequest("method", "url", 100));
            finishedRequests.TryAdd("2", new FinishedRequest("method", "url2", 200));
            finishedRequests.TryAdd("3", new FinishedRequest("method", "url2", 400));
            finishedRequests.TryAdd("4", new FinishedRequest("method", "url2", 800));

            var storage = A.Fake<IRequestsStorage>();
            A.CallTo(() => storage.FinishedRequests).Returns(finishedRequests);

            var metric = new RequestsMedianTimeMetric();

            var actual = metric.GetValue(storage);
            var expected = 300.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsMedianTimeMetric_OddRequestsCount_IsCorrect()
        {
            var finishedRequests = new ConcurrentDictionary<string, FinishedRequest>();
            finishedRequests.TryAdd("1", new FinishedRequest("method", "url", 333));
            finishedRequests.TryAdd("2", new FinishedRequest("method", "url2", 444));
            finishedRequests.TryAdd("3", new FinishedRequest("method", "url2", 555));

            var storage = A.Fake<IRequestsStorage>();
            A.CallTo(() => storage.FinishedRequests).Returns(finishedRequests);

            var metric = new RequestsMedianTimeMetric();

            var actual = metric.GetValue(storage);
            var expected = 444.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}