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

        public async Task SendStartedRequest(Dictionary<string, string> content)
        {
            await Send(_urlForStartedRequest, content);
        }

        public async Task SendFinishedRequest(Dictionary<string, string> content)
        {
            await Send(_urlForFinishedRequest, content);
        }

        private async Task Send(string url, Dictionary<string, string> content)
        {
            await _client.PostAsync(url, new FormUrlEncodedContent(content));
        }
    }
}