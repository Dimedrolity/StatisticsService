using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MainService.Requests;

namespace MainService.Metrics
{
    public class RequestsMedianTimeMetric : Metric<FinishedRequest>
    {
        public override string Name { get; } = "requestsMedianTime";

        protected override string CalculateValue(ICollection<FinishedRequest> requests)
        {
            return (requests.Count == 0
                    ? 0
                    : GetMedian(requests
                        .Select(req => req.ElapsedTimeInMilliseconds)
                        .ToArray()))
                .ToString(CultureInfo.InvariantCulture);
        }

        private static int GetMedian(int[] numbers)
        {
            Array.Sort(numbers);
            var size = numbers.Length;
            var mid = size / 2;
            var median = size % 2 != 0 ? numbers[mid] : (numbers[mid] + numbers[mid - 1]) / 2;

            return median;
        }
    }
}