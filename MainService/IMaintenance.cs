using System.Threading.Tasks;

namespace MainService
{
    public interface IMaintenance
    {
        Task StartAsync();

        void Stop();

        bool IsStopped { get; }
    }
}