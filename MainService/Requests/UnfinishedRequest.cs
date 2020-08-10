namespace MainService.Requests
{
    public class UnfinishedRequest : Request
    {
        public long StartTimeInMilliseconds { get; }

        public UnfinishedRequest(string method, string url, long startTimeInMilliseconds) : base(method, url)
        {
            StartTimeInMilliseconds = startTimeInMilliseconds;
        }
    }
}