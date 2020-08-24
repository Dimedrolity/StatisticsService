using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MiddlewareClassLibrary;
using Newtonsoft.Json;

namespace ServiceOne
{
    public class UdpSender : ISender
    {
        private readonly string _host;
        private readonly int _port;

        public UdpSender(IUdpConfig config)
        {
            _host = config.GetHost();
            _port = config.GetPort();
        }

        public async Task SendStartedRequestAsync(Dictionary<string, string> content)
        {
            content.Add("request-status", "started");
            await SendAsync(_port, content);
        }

        public async Task SendFinishedRequestAsync(Dictionary<string, string> content)
        {
            content.Add("request-status", "finished");
            await SendAsync(_port, content);
        }

        public async Task SendFailedRequestAsync(Dictionary<string, string> content)
        {
            content.Add("request-status", "failed");
            await SendAsync(_port, content);
        }

        private async Task SendAsync(int port, Dictionary<string, string> content)
        {
            using var sender = new UdpClient(_host, port);

            var contentAsString = JsonConvert.SerializeObject(content);
            var data = Encoding.UTF8.GetBytes(contentAsString);
            await sender.SendAsync(data, data.Length);
        }
    }
}