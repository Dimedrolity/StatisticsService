using Microsoft.Extensions.Configuration;

namespace ServiceTwo
{
    public class HttpConfig : IHttpConfig
    {
        private readonly IConfiguration _configuration;

        public HttpConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetHostOfStatisticsService()
        {
            return _configuration["HostOfStatisticsService"];
        }

        public string GetPathForStartedRequest()
        {
            return _configuration["PathForStartedRequest"];
        }

        public string GetPathForFinishedRequest()
        {
            return _configuration["PathForFinishedRequest"];
        }

        public string GetPathForFailedRequest()
        {
            return _configuration["PathForFailedRequest"];
        }
    }
}