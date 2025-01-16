using MinMax.Models;
using System.Collections.Generic;

namespace MinMax.Interfaces
{
    public interface IStatisticsCalculator
    {
        /// <summary>
        /// The input is a collection of read data on events.
        /// The output is ready statistics for each type of event.
        /// </summary>
        IDictionary<string, EventStatistics> CalculateStatistics(IEnumerable<EventData> events);
    }
}
