using Microsoft.Extensions.Configuration;

namespace MainService.Middleware
{
    public class UdpConfig : IUdpConfig
    {
        private readonly IConfiguration _configuration;

        public UdpConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int GetPortForStartedRequest()
        {
            return int.Parse(_configuration["portForStartedRequest_Udp"]);
        }

        public int GetPortForFinishedRequest()
        {
            return int.Parse(_configuration["portForFinishedRequest_Udp"]);
        }
    }
}