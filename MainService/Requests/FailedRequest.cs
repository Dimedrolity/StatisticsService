namespace MainService.Requests
{
    public class FailedRequest : Request
    {
        public long ApproximateTime { get; }

        public FailedRequest(string host, string method, long approximateTime) : base(host, method)
        {
            ApproximateTime = approximateTime;
        }
    }
}