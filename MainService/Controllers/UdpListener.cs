using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MainService.Controllers
{
    public class UdpListener
    {
        private readonly IRequestsCollector _requestsCollector;

        private readonly int _portForStartedRequest;
        private readonly int _portForFinishedRequest;

        private readonly ILogger<UdpListener> _logger;

        public UdpListener(IRequestsCollector requestsCollector, IConfiguration configuration,
            ILogger<UdpListener> logger)
        {
            _requestsCollector = requestsCollector;

            _portForStartedRequest = int.Parse(configuration["portForStartedRequest_Udp"]);
            _portForFinishedRequest = int.Parse(configuration["portForFinishedRequest_Udp"]);

            _logger = logger;
        }

        public async Task Listen()
        {
            var task1 = ListenForStartedRequests();

            var task2 = ListenForFinishedRequests();

            await Task.WhenAll(task1, task2);
        }

        private async Task ListenForStartedRequests()
        {
            using var listener = new UdpClient(_portForStartedRequest);

            while (true)
            {
                var content = await ReceiveContent(listener);

                var guid = content["guid"];
                var host = content["host"];
                var path = content["path"];
                var method = content["method"];
                var startTime = content["time-as-milliseconds-from-unix-epoch"];
                var url = $"{host}/{path}";

                await Task.Run(() => _requestsCollector.SaveStartedRequest(guid, method, url, long.Parse(startTime)));

                _logger.LogInformation($"начал выполнение запрос: {guid} метод: {method} url: {host}/{path}\n" +
                                       $"время начала запроса: {startTime}");
            }
        }

        private static async Task<Dictionary<string, string>> ReceiveContent(UdpClient receiver)
        {
            var data = await receiver.ReceiveAsync();
            var message = Encoding.UTF8.GetString(data.Buffer);
            var content = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
            return content;
        }

        private async Task ListenForFinishedRequests()
        {
            using var listener = new UdpClient(_portForFinishedRequest);

            while (true)
            {
                var content = await ReceiveContent(listener);

                var guid = content["guid"];
                var finishTime = content["time-as-milliseconds-from-unix-epoch"];

                await Task.Run(() => _requestsCollector.SaveFinishedRequest(guid, long.Parse(finishTime)));

                _logger.LogInformation($"выполнился запрос: {guid}\n" +
                                       $"время окончания запроса: {finishTime}");
            }
        }
    }
}