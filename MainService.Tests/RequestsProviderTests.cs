using System.Collections.Concurrent;
using System.Collections.Generic;
using FakeItEasy;
using MainService.Requests;
using NUnit.Framework;

namespace MainService.Tests
{
    public class RequestsProviderTests
    {
        private IRequestsStorage _storage;
        private IRequestsProvider _requestsProvider;

        [SetUp]
        public void Setup()
        {
            _storage = A.Fake<IRequestsStorage>();
            _requestsProvider = new RequestsProvider(_storage);
        }

        [Test]
        public void GetUnfinishedRequestsInHierarchicalStructure()
        {
            var cd = new ConcurrentDictionary<string, UnfinishedRequest>();
            var request1 = new UnfinishedRequest("h1", "m1", 0);
            var request2 = new UnfinishedRequest("h1", "m2", 0);
            var request3 = new UnfinishedRequest("h2", "m1", 0);
            var request4 = new UnfinishedRequest("h2", "m2", 0);
            cd.TryAdd("1", request1);
            cd.TryAdd("2", request2);
            cd.TryAdd("3", request3);
            cd.TryAdd("4", request4);

            A.CallTo(() => _storage.UnfinishedRequests).Returns(cd);

            var actual = _requestsProvider.GetUnfinishedRequestsInHierarchicalStructure();
            var expected = new Dictionary<string, Dictionary<string, List<UnfinishedRequest>>>
            {
                {
                    "h1", new Dictionary<string, List<UnfinishedRequest>>
                    {
                        {"m1", new List<UnfinishedRequest> {request1}},
                        {"m2", new List<UnfinishedRequest> {request2}}
                    }
                },
                {
                    "h2", new Dictionary<string, List<UnfinishedRequest>>
                    {
                        {"m1", new List<UnfinishedRequest> {request3}},
                        {"m2", new List<UnfinishedRequest> {request4}}
                    }
                }
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GetFinishedRequestsInHierarchicalStructure()
        {
            var requests = new ConcurrentDictionary<string, FinishedRequest>();
            var request1 = new FinishedRequest("h1", "m1", 0);
            var request2 = new FinishedRequest("h1", "m2", 0);
            var request3 = new FinishedRequest("h2", "m1", 0);
            var request4 = new FinishedRequest("h2", "m2", 0);
            requests.TryAdd("1", request1);
            requests.TryAdd("2", request2);
            requests.TryAdd("3", request3);
            requests.TryAdd("4", request4);

            A.CallTo(() => _storage.FinishedRequests).Returns(requests);

            var actual = _requestsProvider.GetFinishedRequestsInHierarchicalStructure();
            var expected = new Dictionary<string, Dictionary<string, List<FinishedRequest>>>
            {
                {
                    "h1", new Dictionary<string, List<FinishedRequest>>
                    {
                        {"m1", new List<FinishedRequest> {request1}},
                        {"m2", new List<FinishedRequest> {request2}}
                    }
                },
                {
                    "h2", new Dictionary<string, List<FinishedRequest>>
                    {
                        {"m1", new List<FinishedRequest> {request3}},
                        {"m2", new List<FinishedRequest> {request4}}
                    }
                }
            };

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void GetRequestsWithErrorsInHierarchicalStructure()
        {
            var requests = new ConcurrentBag<FailedRequest>();
            var request1 = new FailedRequest("h1", "m1", 0);
            var request2 = new FailedRequest("h1", "m2", 0);
            var request3 = new FailedRequest("h2", "m1", 0);
            var request4 = new FailedRequest("h2", "m2", 0);
            requests.Add(request1);
            requests.Add(request2);
            requests.Add(request3);
            requests.Add(request4);

            A.CallTo(() => _storage.RequestsWithErrors).Returns(requests);

            var actual = _requestsProvider.GetRequestsWithErrorsInHierarchicalStructure();
            var expected = new Dictionary<string, Dictionary<string, List<FailedRequest>>>
            {
                {
                    "h1", new Dictionary<string, List<FailedRequest>>
                    {
                        {"m1", new List<FailedRequest> {request1}},
                        {"m2", new List<FailedRequest> {request2}}
                    }
                },
                {
                    "h2", new Dictionary<string, List<FailedRequest>>
                    {
                        {"m1", new List<FailedRequest> {request3}},
                        {"m2", new List<FailedRequest> {request4}}
                    }
                }
            };

            CollectionAssert.AreEqual(expected, actual);
        }
        
        [Test]
        public void GetLostUdpPacketsInHierarchicalStructure()
        {
            var requests = new ConcurrentBag<FailedRequest>();
            var request1 = new FailedRequest("h1", "m1", 0);
            var request2 = new FailedRequest("h1", "m2", 0);
            var request3 = new FailedRequest("h2", "m1", 0);
            var request4 = new FailedRequest("h2", "m2", 0);
            requests.Add(request1);
            requests.Add(request2);
            requests.Add(request3);
            requests.Add(request4);

            A.CallTo(() => _storage.LostUdpPackets).Returns(requests);

            var actual = _requestsProvider.GetLostUdpPacketsInHierarchicalStructure();
            var expected = new Dictionary<string, Dictionary<string, List<FailedRequest>>>
            {
                {
                    "h1", new Dictionary<string, List<FailedRequest>>
                    {
                        {"m1", new List<FailedRequest> {request1}},
                        {"m2", new List<FailedRequest> {request2}}
                    }
                },
                {
                    "h2", new Dictionary<string, List<FailedRequest>>
                    {
                        {"m1", new List<FailedRequest> {request3}},
                        {"m2", new List<FailedRequest> {request4}}
                    }
                }
            };

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}