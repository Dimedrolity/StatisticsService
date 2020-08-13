using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MainService.Middleware
{
    public class UdpSender : IRequestSender
    {
        private string _host = "127.0.0.1";

        private readonly int _portForStartedRequest = 7003;
        private readonly int _portForFinishedRequest = 7004;

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