using Microsoft.Extensions.Configuration;

namespace ServiceOne
{
    public class UdpConfig : IUdpConfig
    {
        private readonly IConfiguration _configuration;

        public UdpConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetHost()
        {
            return _configuration["UdpHost"];
        }

        public int GetPort()
        {
            return int.Parse(_configuration["UdpPort"]);
        }
    }
}