using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MainService.ExternalMiddleware
{
    public class HttpSender : IRequestSender
    {
        private readonly HttpClient _client = new HttpClient();

        private readonly string _urlForStartedRequest = "http://localhost:7000/api/requests/request-started";
        private readonly string _urlForFinishedRequest = "http://localhost:7000/api/requests/request-finished";

        public async Task SendStartedRequestAsync(Dictionary<string, string> content)
        {
            await SendAsync(_urlForStartedRequest, content);
        }

        public async Task SendFinishedRequestAsync(Dictionary<string, string> content)
        {
            await SendAsync(_urlForFinishedRequest, content);
        }

        private async Task SendAsync(string url, Dictionary<string, string> content)
        {
            await _client.PostAsync(url, new FormUrlEncodedContent(content));
        }
    }
}