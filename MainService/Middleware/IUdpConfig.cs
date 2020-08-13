namespace MainService.Middleware
{
    public interface IUdpConfig
    {
        public int GetPortForStartedRequest();
        public int GetPortForFinishedRequest();
    }
}