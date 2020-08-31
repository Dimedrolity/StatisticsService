namespace MainService.Requests
{
    public class FinishedRequest : Request
    {
        public int ElapsedTimeInMilliseconds { get; }

        public FinishedRequest(string host, string method, int elapsedTimeInMilliseconds) : base(host, method)
        {
            ElapsedTimeInMilliseconds = elapsedTimeInMilliseconds;
        }
    }
}