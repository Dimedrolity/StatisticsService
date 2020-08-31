namespace MainService.Requests
{
    public class UnfinishedRequest : Request
    {
        public long StartTimeInMilliseconds { get; }

        public UnfinishedRequest(string host, string method, long startTimeInMilliseconds) : base(host, method)
        {
            StartTimeInMilliseconds = startTimeInMilliseconds;
        }
    }
}