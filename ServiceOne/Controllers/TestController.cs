using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ServiceOne.Controllers
{
    [ApiController]
    [Route("api/")]
    public class TestController : ControllerBase
    {
        [HttpGet("get1")]
        public async Task<string> Get()
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync("http://localhost:7002/api/get2");
            return $"ServiceOne and {response}";
        }
    }
}