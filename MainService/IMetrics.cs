namespace MainService
{
    public interface IMetrics
    {
        public int GetUnfinishedRequestsCount();
        public double GetRequestsAverageTime();
    }
}