using System.Collections.Concurrent;
using FakeItEasy;
using MainService.Metrics;
using MainService.Requests;
using NUnit.Framework;

namespace MainService.Tests
{
    public class MetricsTests
    {
        private IRequestsStorage _storage;

        [SetUp]
        public void Setup()
        {
            _storage = A.Fake<IRequestsStorage>();
        }

        [Test]
        public void UnfinishedRequestsMetric_IsCorrect()
        {
            var unfinishedRequests = new ConcurrentDictionary<string, UnfinishedRequest>();
            unfinishedRequests.TryAdd("123", new UnfinishedRequest("method", "url", 0));
            unfinishedRequests.TryAdd("456", new UnfinishedRequest("method", "url2", 0));

            A.CallTo(() => _storage.UnfinishedRequests).Returns(unfinishedRequests);

            var metric = new UnfinishedRequestsMetric();

            var actual = metric.GetValue(_storage);
            var expected = _storage.UnfinishedRequests.Count.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FinishedRequestsCountMetric_IsCorrect()
        {
            var unfinishedRequests = new ConcurrentDictionary<string, FinishedRequest>();
            unfinishedRequests.TryAdd("123", new FinishedRequest("method", "url", 0));
            unfinishedRequests.TryAdd("456", new FinishedRequest("method", "url2", 0));

            A.CallTo(() => _storage.FinishedRequests).Returns(unfinishedRequests);

            var metric = new FinishedRequestsMetric();

            var actual = metric.GetValue(_storage);
            var expected = _storage.FinishedRequests.Count.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsWithErrorsMetric_IsCorrect()
        {
            var unfinishedRequests = new ConcurrentBag<FailedRequest>
            {
                new FailedRequest("method", "url", 0),
                new FailedRequest("method", "url2", 0)
            };

            A.CallTo(() => _storage.RequestsWithErrors).Returns(unfinishedRequests);

            var metric = new RequestsWithErrorsMetric();

            var actual = metric.GetValue(_storage);
            var expected = _storage.RequestsWithErrors.Count.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsAverageTimeMetric_IsCorrect()
        {
            var finishedRequests = new ConcurrentDictionary<string, FinishedRequest>();
            finishedRequests.TryAdd("1", new FinishedRequest("method", "url", 100));
            finishedRequests.TryAdd("2", new FinishedRequest("method", "url2", 200));
            finishedRequests.TryAdd("3", new FinishedRequest("method", "url2", 300));

            A.CallTo(() => _storage.FinishedRequests).Returns(finishedRequests);

            var metric = new RequestsAverageTimeMetric();

            var actual = metric.GetValue(_storage);
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

            A.CallTo(() => _storage.FinishedRequests).Returns(finishedRequests);

            var metric = new RequestsMinTimeMetric();

            var actual = metric.GetValue(_storage);
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

            A.CallTo(() => _storage.FinishedRequests).Returns(finishedRequests);

            var metric = new RequestsMaxTimeMetric();

            var actual = metric.GetValue(_storage);
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

            A.CallTo(() => _storage.FinishedRequests).Returns(finishedRequests);

            var metric = new RequestsMedianTimeMetric();

            var actual = metric.GetValue(_storage);
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

            A.CallTo(() => _storage.FinishedRequests).Returns(finishedRequests);

            var metric = new RequestsMedianTimeMetric();

            var actual = metric.GetValue(_storage);
            var expected = 444.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}