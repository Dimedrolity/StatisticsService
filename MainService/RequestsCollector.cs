﻿using System.Collections.Generic;
using System.Linq;
using MainService.Requests;

namespace MainService
{
    public class RequestsCollector : IRequestsCollector
    {
        public HashSet<UnfinishedRequest> UnfinishedRequests { get; }
        public HashSet<FinishedRequest> FinishedRequests { get; }
        
        private readonly object _lockObject = new object();

        public RequestsCollector()
        {
            UnfinishedRequests = new HashSet<UnfinishedRequest>();
            FinishedRequests = new HashSet<FinishedRequest>();
        }

        public void SaveStartedRequest(string method, string url, long startTime)
        {
            lock (_lockObject)
            {
                var request = new UnfinishedRequest(method, url, startTime);
                UnfinishedRequests.Add(request);
            }
        }

        public void SaveFinishedRequest(string method, string url, long finish)
        {
            lock (_lockObject)
            {
                var unfinishedRequest =
                    UnfinishedRequests.First(request => request.Url == url && request.Method == method);
                UnfinishedRequests.Remove(unfinishedRequest);

                var finishedRequest =
                    new FinishedRequest(method, url, (int) (finish - unfinishedRequest.StartTimeInMilliseconds));
                FinishedRequests.Add(finishedRequest);
            }
        }
    }
}