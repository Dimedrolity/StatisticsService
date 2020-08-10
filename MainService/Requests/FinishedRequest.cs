namespace MainService.Requests
{
    public class FinishedRequest : Request
    {
        public long ElapsedTimeInMilliseconds { get; }

        public FinishedRequest(string method, string url, long elapsedTimeInMilliseconds) : base(method, url)
        {
            ElapsedTimeInMilliseconds = elapsedTimeInMilliseconds;
        }
    }
}