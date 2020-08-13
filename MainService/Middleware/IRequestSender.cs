using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainService.Middleware
{
    public interface IRequestSender
    {
        public Task SendStartedRequest(Dictionary<string, string> content);
        public Task SendFinishedRequest(Dictionary<string, string> content);
    }
}