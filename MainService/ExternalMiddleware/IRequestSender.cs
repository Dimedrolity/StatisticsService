using System.Collections.Generic;
using System.Threading.Tasks;

namespace MainService.ExternalMiddleware
{
    public interface IRequestSender
    {
        Task SendStartedRequestAsync(Dictionary<string, string> content);
        Task SendFinishedRequestAsync(Dictionary<string, string> content);
    }
}