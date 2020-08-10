using NUnit.Framework;

namespace MainService.Tests
{
    public class Tests
    {
        private IRequestsCollector _collector;

        [SetUp]
        public void Setup()
        {
            _collector = new RequestsCollector();
        }

        [Test]
        public void SaveStartedRequest_AddsRequestToUnfinishedRequests()
        {
            _collector.SaveStartedRequest("method", "url", 0);

            var actual = _collector.UnfinishedRequests.Count;
            var expected = 1;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SaveFinishedRequest_RemovesRequestFromUnfinishedRequests()
        {
            _collector.SaveStartedRequest("method", "url", 0);
            _collector.SaveFinishedRequest("method", "url", 1);
            
            var actual = _collector.UnfinishedRequests.Count;
            var expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SaveFinishedRequest_AddsRequestToFinishedRequests()
        {
            _collector.SaveStartedRequest("method", "url", 0);
            _collector.SaveFinishedRequest("method", "url", 1);
            
            var actual = _collector.FinishedRequests.Count;
            var expected = 1;

            Assert.AreEqual(expected, actual);
        }
    }
}