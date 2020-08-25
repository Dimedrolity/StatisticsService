using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MiddlewareClassLibrary;

namespace ServiceTwo
{
    public class HttpRequestSender : IRequestSender
    {
        private readonly HttpClient _client = new HttpClient();

        private readonly string _urlForStartedRequest;
        private readonly string _urlForFinishedRequest;
        private readonly string _urlForFailedRequest;

        public HttpRequestSender(IHttpConfig config)
        {
            var host = config.GetHostOfStatisticsService();

            var pathForStartedRequest = config.GetPathForStartedRequest();
            _urlForStartedRequest = $"{host}/{pathForStartedRequest}";

            var pathForFinishedRequest = config.GetPathForFinishedRequest();
            _urlForFinishedRequest = $"{host}/{pathForFinishedRequest}";

            var pathForFailedRequest = config.GetPathForFailedRequest();
            _urlForFailedRequest = $"{host}/{pathForFailedRequest}";
        }

        public async Task SendStartedRequestAsync(Dictionary<string, string> content)
        {
            await SendAsync(_urlForStartedRequest, content);
        }

        public async Task SendFinishedRequestAsync(Dictionary<string, string> content)
        {
            await SendAsync(_urlForFinishedRequest, content);
        }

        public async Task SendFailedRequestAsync(Dictionary<string, string> content)
        {
            await _client.PostAsync(_urlForFailedRequest, new FormUrlEncodedContent(content));
        }

        private async Task SendAsync(string url, Dictionary<string, string> content)
        {
            await _client.PostAsync(url, new FormUrlEncodedContent(content));
        }
    }
}