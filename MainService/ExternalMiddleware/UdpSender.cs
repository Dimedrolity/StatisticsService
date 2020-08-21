using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MainService.ExternalMiddleware
{
    public class UdpSender : IRequestSender
    {
        private readonly string _host;
        private readonly int _port;

        public UdpSender(IUdpConfig config)
        {
            _host = config.GetHost();
            _port = config.GetPort();
        }

        public async Task SendStartedRequest(Dictionary<string, string> content)
        {
            content.Add("request-started-or-finished", "started");
            await Send(_port, content);
        }

        public async Task SendFinishedRequest(Dictionary<string, string> content)
        {
            content.Add("request-started-or-finished", "finished");
            await Send(_port, content);
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