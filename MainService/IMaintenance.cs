using System.Threading;
using System.Threading.Tasks;

namespace MainService
{
    public interface IMaintenance
    {
        Task Start(CancellationTokenSource tokenSource);

        void Stop();

        bool IsStopped { get; }
    }
}