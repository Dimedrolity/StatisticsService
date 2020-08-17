namespace MainService.Middleware
{
    public interface IUdpConfig
    {
        public string GetHost();
        public int GetPort();
    }
}