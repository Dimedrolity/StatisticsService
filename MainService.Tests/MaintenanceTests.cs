using System.Threading;
using FakeItEasy;
using MainService.Controllers;
using NUnit.Framework;

namespace MainService.Tests
{
    public class MaintenanceTests
    {
        [Test]
        public void Start_ActivatesUdpListener()
        {
            var requestsCleaner = A.Fake<IOldRequestsCleaner>();
            var udpListener = A.Fake<IUdpListener>();
            var maintenance = new Maintenance(requestsCleaner, udpListener);
            maintenance.Start(new CancellationTokenSource());

            A.CallTo(() => udpListener.Listen(A<CancellationToken>._))
                .WithAnyArguments().MustHaveHappened(1, Times.Exactly);
        }

        [Test]
        public void Start_ActivatesOldRequestsCleaner()
        {
            var requestsCleaner = A.Fake<IOldRequestsCleaner>();
            var udpListener = A.Fake<IUdpListener>();
            var maintenance = new Maintenance(requestsCleaner, udpListener);
            maintenance.Start(new CancellationTokenSource());

            A.CallTo(() => requestsCleaner.MoveOldRequestsToFailedRequests(A<CancellationToken>._))
                .WithAnyArguments().MustHaveHappened(1, Times.Exactly);
        }

        [Test]
        public void Stop_CancelsToken()
        {
            var requestsCleaner = A.Fake<IOldRequestsCleaner>();
            var udpListener = A.Fake<IUdpListener>();
            var maintenance = new Maintenance(requestsCleaner, udpListener);

            var tokenSource = new CancellationTokenSource();
            maintenance.Start(tokenSource);
            maintenance.Stop();

            Assert.IsTrue(tokenSource.IsCancellationRequested);
        }
    }
}