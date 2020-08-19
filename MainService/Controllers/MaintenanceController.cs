using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenance _maintenance;

        public MaintenanceController(IMaintenance maintenance)
        {
            _maintenance = maintenance;
        }

        [HttpGet("start")]
        public async Task<IActionResult> Start()
        {
            _maintenance.Start(new CancellationTokenSource());
            await Task.Delay(1_000);

            return _maintenance.IsStopped ? StatusCode(403) : StatusCode(200);
        }

        [HttpGet("stop")]
        public async Task<IActionResult> Stop()
        {
            _maintenance.Stop();
            await Task.Delay(1_000);
            
            return _maintenance.IsStopped ? StatusCode(200) : StatusCode(403);
        }
    }
}