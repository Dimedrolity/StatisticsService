namespace MainService.Requests
{
    public class FailedRequest : Request
    {
        public long ApproximateTime { get; }

        public FailedRequest(string method, string url, long approximateTime) : base(method, url)
        {
            ApproximateTime = approximateTime;
        }
    }
}