﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace MiddlewareClassLibrary
{
    public interface ISender
    {
        Task SendStartedRequestAsync(Dictionary<string, string> content);
        Task SendFinishedRequestAsync(Dictionary<string, string> content);
        Task SendFailedRequestAsync(Dictionary<string, string> content);
    }
}