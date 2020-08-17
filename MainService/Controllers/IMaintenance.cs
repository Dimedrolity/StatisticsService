using System.Threading.Tasks;

namespace MainService.Controllers
{
    public interface IMaintenance
    {
        public Task Start();

        public void Finish();
    }
}