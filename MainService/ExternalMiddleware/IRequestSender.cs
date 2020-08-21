using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainService.ExternalMiddleware
{
    public interface IRequestSender
    {
        Task SendStartedRequest(Dictionary<string, string> content);
        Task SendFinishedRequest(Dictionary<string, string> content);
    }
}