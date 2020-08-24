namespace ServiceTwo
{
    public interface IHttpConfig
    {
        string GetHostOfStatisticsService();
        string GetPathForStartedRequest();
        string GetPathForFinishedRequest();
        string GetPathForFailedRequest();
    }
}