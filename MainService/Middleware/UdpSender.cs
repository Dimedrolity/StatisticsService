using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MainService.Middleware
{
    public class UdpSender : IRequestSender
    {
        private readonly string _host = "127.0.0.1";

        private readonly int _portForStartedRequest;
        private readonly int _portForFinishedRequest;

        public UdpSender(IConfiguration configuration)
        {
            _portForStartedRequest = int.Parse(configuration["portForStartedRequest_Udp"]);
            _portForFinishedRequest = int.Parse(configuration["portForFinishedRequest_Udp"]);
        }

        public async Task SendStartedRequest(Dictionary<string, string> content)
        {
            await Send(_portForStartedRequest, content);
        }

        public async Task SendFinishedRequest(Dictionary<string, string> content)
        {
            await Send(_portForFinishedRequest, content);
        }

        private async Task Send(int port, Dictionary<string, string> content)
        {
            using var sender = new UdpClient(_host, port);

            var contentAsString = JsonConvert.SerializeObject(content);
            var data = Encoding.UTF8.GetBytes(contentAsString);
            await sender.SendAsync(data, data.Length);
        }
    }
}