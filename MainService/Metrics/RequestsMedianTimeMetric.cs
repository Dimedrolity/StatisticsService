using System;
using System.Globalization;
using System.Linq;

namespace MainService.Metrics
{
    public class RequestsMedianTimeMetric : Metric
    {
        public override string Name { get; } = "requestsMedianTime";

        public override string GetValue(IRequestsStorage storage)
        {
            return (storage.FinishedRequests.Count == 0
                    ? 0
                    : GetMedian(storage.FinishedRequests.Values
                        .Select(req => req.ElapsedTimeInMilliseconds)
                        .ToArray()))
                .ToString(CultureInfo.InvariantCulture);
        }

        public static int GetMedian(int[] numbers)
        {
            Array.Sort(numbers);
            var size = numbers.Length;
            var mid = size / 2;
            var median = size % 2 != 0 ? numbers[mid] : (numbers[mid] + numbers[mid - 1]) / 2;

            return median;
        }
    }
}