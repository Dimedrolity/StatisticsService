namespace MainService.Requests
{
    public abstract class Request
    {
        public string Method { get; }

        public string Url { get; }

        protected Request(string method, string url)
        {
            Method = method;
            Url = url;
        }

        public override int GetHashCode()
        {
            return Method.GetHashCode() + Url.GetHashCode();
        }
    }
}