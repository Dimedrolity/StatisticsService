using System.Threading.Tasks;

namespace MainService
{
    public interface IMaintenance
    {
        Task Start();

        void Stop();

        bool IsStopped { get; }
    }
}