using System.Collections.Generic;
using FakeItEasy;
using MainService.Metrics;
using MainService.Requests;
using NUnit.Framework;

namespace MainService.Tests
{
    public class MetricsTests
    {
        private IRequestsProvider _requestsProvider;

        [SetUp]
        public void Setup()
        {
            _requestsProvider = A.Fake<IRequestsProvider>();
        }

        [Test]
        public void UnfinishedRequestsMetric_IsCorrect()
        {
            var request1 = new UnfinishedRequest(null, null, 0);
            var request2 = new UnfinishedRequest(null, null, 0);

            A.CallTo(() => _requestsProvider.GetUnfinishedRequestsInHierarchicalStructure())
                .Returns(
                    new Dictionary<string, Dictionary<string, List<UnfinishedRequest>>>
                    {
                        {
                            "url", new Dictionary<string, List<UnfinishedRequest>>()
                            {
                                {
                                    "method", new List<UnfinishedRequest>()
                                    {
                                        request1, request2
                                    }
                                }
                            }
                        }
                    });

            var metric = new UnfinishedRequestsCountMetric();

            var requests =
                _requestsProvider.GetUnfinishedRequestsInHierarchicalStructure();

            var statistics = metric.GetStatistics(requests);

            var actual = statistics["url"]["method"][metric.Name];
            var expected = 2.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FinishedRequestsCountMetric_IsCorrect()
        {
            var request1 = new FinishedRequest(null, null, 0);
            var request2 = new FinishedRequest(null, null, 0);

            A.CallTo(() => _requestsProvider.GetFinishedRequestsInHierarchicalStructure())
                .Returns(
                    new Dictionary<string, Dictionary<string, List<FinishedRequest>>>
                    {
                        {
                            "url", new Dictionary<string, List<FinishedRequest>>()
                            {
                                {
                                    "method", new List<FinishedRequest>()
                                    {
                                        request1, request2
                                    }
                                }
                            }
                        }
                    });

            var metric = new FinishedRequestsCountMetric();

            var requests =
                _requestsProvider.GetFinishedRequestsInHierarchicalStructure();

            var statistics = metric.GetStatistics(requests);

            var actual = statistics["url"]["method"][metric.Name];
            var expected = 2.ToString();

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RequestsWithErrorsMetric_IsCorrect()
        {
            var request1 = new FailedRequest(null, null, 0);
            var request2 = new FailedRequest(null, null, 0);

            A.CallTo(() => _requestsProvider.GetRequestsWithErrorsInHierarchicalStructure())
                .Returns(
                    new Dictionary<string, Dictionary<string, List<FailedRequest>>>
                    {
                        {
                            "url", new Dictionary<string, List<FailedRequest>>()
                            {
                                {
                                    "method", new List<FailedRequest>()
                                    {
                                        request1, request2
                                    }
                                }
                            }
                        }
                    });

            var metric = new RequestsWithErrorsCountMetric();

            var requests =
                _requestsProvider.GetRequestsWithErrorsInHierarchicalStructure();

            var statistics = metric.GetStatistics(requests);

            var actual = statistics["url"]["method"][metric.Name];
            var expected = 2.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsAverageTimeMetric_IsCorrect()
        {
            var request1 = new FinishedRequest(null, null, 100);
            var request2 = new FinishedRequest(null, null, 200);
            var request3 = new FinishedRequest(null, null, 300);

            A.CallTo(() => _requestsProvider.GetFinishedRequestsInHierarchicalStructure())
                .Returns(
                    new Dictionary<string, Dictionary<string, List<FinishedRequest>>>
                    {
                        {
                            "url", new Dictionary<string, List<FinishedRequest>>()
                            {
                                {
                                    "method", new List<FinishedRequest>()
                                    {
                                        request1, request2, request3
                                    }
                                }
                            }
                        }
                    });

            var metric = new RequestsAverageTimeMetric();

            var requests =
                _requestsProvider.GetFinishedRequestsInHierarchicalStructure();

            var statistics = metric.GetStatistics(requests);

            var actual = statistics["url"]["method"][metric.Name];
            var expected = 200.ToString();

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RequestsMinTimeMetric_IsCorrect()
        {
            var request1 = new FinishedRequest(null, null, 100);
            var request2 = new FinishedRequest(null, null, 200);
            var request3 = new FinishedRequest(null, null, 300);

            A.CallTo(() => _requestsProvider.GetFinishedRequestsInHierarchicalStructure())
                .Returns(
                    new Dictionary<string, Dictionary<string, List<FinishedRequest>>>
                    {
                        {
                            "url", new Dictionary<string, List<FinishedRequest>>()
                            {
                                {
                                    "method", new List<FinishedRequest>()
                                    {
                                        request1, request2, request3
                                    }
                                }
                            }
                        }
                    });

            var metric = new RequestsMinTimeMetric();

            var requests =
                _requestsProvider.GetFinishedRequestsInHierarchicalStructure();

            var statistics = metric.GetStatistics(requests);

            var actual = statistics["url"]["method"][metric.Name];
            var expected = 100.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsMaxTimeMetric_IsCorrect()
        {
            var request1 = new FinishedRequest(null, null, 100);
            var request2 = new FinishedRequest(null, null, 200);
            var request3 = new FinishedRequest(null, null, 300);

            A.CallTo(() => _requestsProvider.GetFinishedRequestsInHierarchicalStructure())
                .Returns(
                    new Dictionary<string, Dictionary<string, List<FinishedRequest>>>
                    {
                        {
                            "url", new Dictionary<string, List<FinishedRequest>>()
                            {
                                {
                                    "method", new List<FinishedRequest>()
                                    {
                                        request1, request2, request3
                                    }
                                }
                            }
                        }
                    });

            var metric = new RequestsMaxTimeMetric();

            var requests =
                _requestsProvider.GetFinishedRequestsInHierarchicalStructure();

            var statistics = metric.GetStatistics(requests);

            var actual = statistics["url"]["method"][metric.Name];
            var expected = 300.ToString();

            Assert.AreEqual(expected, actual);
        }


        [Test]
        public void RequestsMedianTimeMetric_EvenRequestsCount_IsCorrect()
        {
            var request1 = new FinishedRequest(null, null, 100);
            var request2 = new FinishedRequest(null, null, 200);
            var request3 = new FinishedRequest(null, null, 400);
            var request4 = new FinishedRequest(null, null, 800);

            A.CallTo(() => _requestsProvider.GetFinishedRequestsInHierarchicalStructure())
                .Returns(
                    new Dictionary<string, Dictionary<string, List<FinishedRequest>>>
                    {
                        {
                            "url", new Dictionary<string, List<FinishedRequest>>()
                            {
                                {
                                    "method", new List<FinishedRequest>()
                                    {
                                        request1, request2, request3, request4
                                    }
                                }
                            }
                        }
                    });

            var metric = new RequestsMedianTimeMetric();

            var requests =
                _requestsProvider.GetFinishedRequestsInHierarchicalStructure();

            var statistics = metric.GetStatistics(requests);

            var actual = statistics["url"]["method"][metric.Name];
            var expected = 300.ToString();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void RequestsMedianTimeMetric_OddRequestsCount_IsCorrect()
        {
            var request1 = new FinishedRequest(null, null, 333);
            var request2 = new FinishedRequest(null, null, 444);
            var request3 = new FinishedRequest(null, null, 555);

            A.CallTo(() => _requestsProvider.GetFinishedRequestsInHierarchicalStructure())
                .Returns(
                    new Dictionary<string, Dictionary<string, List<FinishedRequest>>>
                    {
                        {
                            "url", new Dictionary<string, List<FinishedRequest>>()
                            {
                                {
                                    "method", new List<FinishedRequest>()
                                    {
                                        request1, request2, request3
                                    }
                                }
                            }
                        }
                    });

            var metric = new RequestsMedianTimeMetric();

            var requests =
                _requestsProvider.GetFinishedRequestsInHierarchicalStructure();

            var statistics = metric.GetStatistics(requests);

            var actual = statistics["url"]["method"][metric.Name];
            var expected = 444.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}