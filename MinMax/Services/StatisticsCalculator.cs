using MinMax.Interfaces;
using MinMax.Models;
using System.Collections.Generic;
using System.Linq;

namespace MinMax.Services
{
    public class StatisticsCalculator : IStatisticsCalculator
    {
        public IDictionary<string, EventStatistics> CalculateStatistics(IEnumerable<EventData> events)
        {
            var EventsGroupedByName = events
                .GroupBy(ev => ev.Name)
                .ToDictionary(grp => grp.Key, grp => grp.ToList());

            var result = new Dictionary<string, EventStatistics>();

            foreach (var group in EventsGroupedByName)
            {
                string name = group.Key;
                var list = group.Value;

                int minTime         = list.Min(ev => ev.Duration);
                int maxTime         = list.Max(ev => ev.Duration);
                double averageTime  = list.Average(ev => ev.Duration);
                int count           = list.Count;

                result[name] = new EventStatistics
                {
                    MinTime     = minTime,
                    MaxTime     = maxTime,
                    AverageTime = averageTime,
                    Count       = count
                };
            }

            return result;
        }
    }

}
