namespace MainService.Requests
{
    public class FinishedRequest : Request
    {
        public int ElapsedTimeInMilliseconds { get; }

        public FinishedRequest(string method, string url, int elapsedTimeInMilliseconds) : base(method, url)
        {
            ElapsedTimeInMilliseconds = elapsedTimeInMilliseconds;
        }
    }
}