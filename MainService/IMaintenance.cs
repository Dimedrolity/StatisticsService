using System.Threading;
using System.Threading.Tasks;

namespace MainService
{
    public interface IMaintenance
    {
        public Task Start(CancellationTokenSource tokenSource);

        public void Stop();
    }
}