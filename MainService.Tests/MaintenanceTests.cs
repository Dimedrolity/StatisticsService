using System.Threading;
using FakeItEasy;
using MainService.Controllers;
using NUnit.Framework;

namespace MainService.Tests
{
    public class MaintenanceTests
    {
        private IOldRequestsCleaner _requestsCleaner;
        private IUdpListener _udpListener;
        private Maintenance _maintenance;

        [SetUp]
        public void Setup()
        {
            _requestsCleaner = A.Fake<IOldRequestsCleaner>();
            _udpListener = A.Fake<IUdpListener>();
            _maintenance = new Maintenance(_requestsCleaner, _udpListener);
        }
        
        [Test]
        public void Start_ActivatesUdpListener()
        {
            _maintenance.Start(new CancellationTokenSource());

            A.CallTo(() => _udpListener.Listen(A<CancellationToken>._))
                .WithAnyArguments().MustHaveHappened(1, Times.Exactly);
        }

        [Test]
        public void Start_ActivatesOldRequestsCleaner()
        {
            _maintenance.Start(new CancellationTokenSource());

            A.CallTo(() => _requestsCleaner.MoveOldRequestsToFailedRequests(A<CancellationToken>._))
                .WithAnyArguments().MustHaveHappened(1, Times.Exactly);
        }

        [Test]
        public void Stop_CancelsToken()
        {
            var tokenSource = new CancellationTokenSource();
            _maintenance.Start(tokenSource);
            _maintenance.Stop();

            Assert.IsTrue(tokenSource.IsCancellationRequested);
        }
    }
}