namespace MainService.Middleware
{
    public interface IUdpConfig
    {
        string GetHost();
        int GetPort();
    }
}