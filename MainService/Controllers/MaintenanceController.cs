using System;
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
        public IActionResult Start()
        {
            _maintenance.StartAsync();
            return Ok("service started");
        }

        [HttpGet("stop")]
        public IActionResult Stop()
        {
            _maintenance.Stop();
            return Ok("service stopped");
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            try
            {
                throw new ArgumentException("first");
            }
            catch (Exception e)
            {
                throw new ApplicationException("second", e);
            }
        }
    }
}