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
            return Ok("started");
        }

        [HttpGet("stop")]
        public async Task<IActionResult> Stop()
        {
            _maintenance.Stop();
            return Ok("stopped");
        }
    }
}