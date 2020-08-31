using NUnit.Framework;

namespace MainService.Tests
{
    public class Tests
    {
        private IRequestsStorage _storage;

        [SetUp]
        public void Setup()
        {
            _storage = new RequestsStorage();
        }

        [Test]
        public void SaveStartedRequest_AddsRequestToUnfinishedRequests()
        {
            _storage.SaveStartedRequest("123", "url", "method", 0);

            var actual = _storage.UnfinishedRequests.Count;
            var expected = 1;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SaveFinishedRequest_RemovesRequestFromUnfinishedRequests()
        {
            _storage.SaveStartedRequest("123", "url", "method", 0);
            _storage.SaveFinishedRequest("123", "url", "method", 1);

            var actual = _storage.UnfinishedRequests.Count;
            var expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SaveFinishedRequest_AddsRequestToFinishedRequests()
        {
            _storage.SaveStartedRequest("123", "url", "method", 0);
            _storage.SaveFinishedRequest("123", "url", "method", 1);
        
            var actual = _storage.FinishedRequests.Count;
            var expected = 1;
        
            Assert.AreEqual(expected, actual);
        }
    }
}