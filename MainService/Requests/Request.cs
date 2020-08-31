namespace MainService.Requests
{
    public abstract class Request
    {
        public string Method { get; }

        public string Host { get; }

        protected Request(string host, string method)
        {
            Method = method;
            Host = host;
        }

        public override int GetHashCode()
        {
            return Method.GetHashCode() + Host.GetHashCode();
        }
    }
}