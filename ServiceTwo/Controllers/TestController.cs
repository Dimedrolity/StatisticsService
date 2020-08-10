using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ServiceTwo.Controllers
{
    [ApiController]
    [Route("api/")]
    public class TestController : ControllerBase
    {
        [HttpGet("get2")]
        public async Task<string> Get()
        {
            await Task.Delay(10_000);
            return "ServiceTwo";
        }
    }
}