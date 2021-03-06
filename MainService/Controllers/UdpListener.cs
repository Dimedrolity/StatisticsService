﻿using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task ListenAsync(CancellationToken token)
        {
            using var listener = new UdpClient(_port);

            while (!token.IsCancellationRequested)
            {
                var content = await ReceiveContentAsync(listener, token);

                switch (content["request-status"])
                {
                    case "started":
                        await ParseContentAndSaveStartedRequestAsync(content);
                        break;
                    case "finished":
                        await ParseContentAndSaveFinishedRequestAsync(content);
                        break;
                    case "failed":
                        await ParseContentAndSaveFailedRequestAsync(content);
                        break;
                }
            }
        }

        private static async Task<Dictionary<string, string>> ReceiveContentAsync(UdpClient receiver,
            CancellationToken token)
        {
            var data = await receiver.ReceiveAsync();

            token.ThrowIfCancellationRequested();

            var message = Encoding.UTF8.GetString(data.Buffer);
            var content = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
            return content;
        }

        private async Task ParseContentAndSaveStartedRequestAsync(IReadOnlyDictionary<string, string> content)
        {
            var guid = content["guid"];
            var host = content["host"];
            var method = content["method"];
            var startTime = content["start-time-as-milliseconds-from-unix-epoch"];


            await Task.Run(
                () => _requestsStorage.SaveStartedRequest(guid, host, method, long.Parse(startTime)));

            _logger.LogInformation($"начал выполнение запрос: {guid} метод: {method} url: {host}\n" +
                                   $"время начала запроса: {startTime}");
        }

        private async Task ParseContentAndSaveFinishedRequestAsync(IReadOnlyDictionary<string, string> content)
        {
            var guid = content["guid"];
            var host = content["host"];
            var method = content["method"];
            var finishTime = content["finish-time-as-milliseconds-from-unix-epoch"];

            await Task.Run(() => _requestsStorage.SaveFinishedRequest(guid, host, method, long.Parse(finishTime)));

            _logger.LogInformation($"выполнился запрос: {guid}\n" +
                                   $"время окончания запроса: {finishTime}");
        }

        private async Task ParseContentAndSaveFailedRequestAsync(IReadOnlyDictionary<string, string> content)
        {
            var guid = content["guid"];
            var host = content["host"];
            var method = content["method"];
            var failTime = content["fail-time-as-milliseconds-from-unix-epoch"];

            await Task.Run(() => { _requestsStorage.SaveRequestWithError(guid, host, method, long.Parse(failTime)); });

            _logger.LogInformation($"запрос завершился с ошибкой {guid}\n" +
                                   $"время ошибки: {failTime}");
        }
    }
}