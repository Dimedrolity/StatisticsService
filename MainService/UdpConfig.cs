using Microsoft.Extensions.Configuration;

namespace MainService
{
    public class UdpConfig : IUdpConfig
    {
        private readonly IConfiguration _configuration;

        public UdpConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GetPort()
        {
            return int.Parse(_configuration["UdpPort"]);
        }
    }
}