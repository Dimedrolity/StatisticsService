using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MainService.Middleware;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MainService.Controllers
{
    public class UdpListener : IUdpListener
    {
        private readonly IRequestsStorage _requestsStorage;

        private readonly int _port;

        private readonly ILogger<UdpListener> _logger;

        public UdpListener(IRequestsStorage requestsStorage, IUdpConfig config, ILogger<UdpListener> logger)
        {
            _requestsStorage = requestsStorage;

            _port = config.GetPort();

            _logger = logger;
        }

        public async Task Listen(CancellationToken token)
        {
            using var listener = new UdpClient(_port);

            while (!token.IsCancellationRequested)
            {
                var content = await ReceiveContent(listener, token);

                switch (content["request-started-or-finished"])
                {
                    case "started":
                        await ParseContentAndSaveStartedRequest(content);
                        break;
                    case "finished":
                        await ParseContentAndSaveFinishedRequest(content);
                        break;
                }
            }
        }

        private static async Task<Dictionary<string, string>> ReceiveContent(UdpClient receiver,
            CancellationToken token)
        {
            var data = await receiver.ReceiveAsync();

            token.ThrowIfCancellationRequested();

            var message = Encoding.UTF8.GetString(data.Buffer);
            var content = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
            return content;
        }

        private async Task ParseContentAndSaveStartedRequest(IReadOnlyDictionary<string, string> content)
        {
            var guid = content["guid"];
            var host = content["host"];
            var path = content["path"];
            var method = content["method"];
            var startTime = content["time-as-milliseconds-from-unix-epoch"];
            var url = $"{host}/{path}";

            await Task.Run(
                () => _requestsStorage.SaveStartedRequest(guid, method, url, long.Parse(startTime)));

            _logger.LogInformation($"начал выполнение запрос: {guid} метод: {method} url: {host}/{path}\n" +
                                   $"время начала запроса: {startTime}");
        }

        private async Task ParseContentAndSaveFinishedRequest(IReadOnlyDictionary<string, string> content)
        {
            var guid = content["guid"];
            var finishTime = content["time-as-milliseconds-from-unix-epoch"];

            await Task.Run(() => _requestsStorage.SaveFinishedRequest(guid, long.Parse(finishTime)));

            _logger.LogInformation($"выполнился запрос: {guid}\n" +
                                   $"время окончания запроса: {finishTime}");
        }
    }
}