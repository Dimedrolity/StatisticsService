using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MiddlewareClassLibrary;

namespace ServiceTwo
{
    public class HttpSender : ISender
    {
        private readonly HttpClient _client = new HttpClient();

        private readonly string _urlForStartedRequest = "http://localhost:7000/api/requests/request-started";
        private readonly string _urlForFinishedRequest = "http://localhost:7000/api/requests/request-finished";
        private readonly string _urlForFailedRequest = "http://localhost:7000/api/requests/request-failed";

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