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
            _maintenance.StartAsync();

            A.CallTo(() => _udpListener.ListenAsync(A<CancellationToken>._))
                .WithAnyArguments().MustHaveHappened(1, Times.Exactly);
        }

        [Test]
        public void Start_ActivatesOldRequestsCleaner()
        {
            _maintenance.StartAsync();

            A.CallTo(() => _requestsCleaner.MoveOldRequestsToFailedRequestsAsync(A<CancellationToken>._))
                .WithAnyArguments().MustHaveHappened(1, Times.Exactly);
        }

        [Test]
        public void Stop_CancelsToken()
        {
            _maintenance.StartAsync();
            _maintenance.Stop();

            Assert.IsTrue(_maintenance.IsStopped);
        }
    }
}