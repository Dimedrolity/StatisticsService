namespace MainService.ExternalMiddleware
{
    public interface IUdpConfig
    {
        string GetHost();
        int GetPort();
    }
}