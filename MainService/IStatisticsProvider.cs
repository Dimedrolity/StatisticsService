namespace MainService
{
    public interface IStatisticsProvider
    {
        string GetStatistics();
        string GetStatistics(string host);
        string GetStatistics(string host, string method);
    }
}