using System.Threading.Tasks;

namespace MainService
{
    public interface IMaintenance
    {
        public Task Start();

        public void Stop();
    }
}