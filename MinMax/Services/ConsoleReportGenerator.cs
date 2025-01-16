using MinMax.Interfaces;
using MinMax.Models;
using System;
using System.Collections.Generic;

namespace MinMax.Services
{
    public class ConsoleReportGenerator : IReportGenerator
    {
        public void GenerateReport(IDictionary<string, EventStatistics> statistics)
        {
            // Output Header
            Console.WriteLine(
                "{0,-40} {1,10} {2,10} {3,10} {4,15}",
                "Event", "Min", "Max", "Average", "Count"
            );

            // Output Each Event's Statistics
            foreach (var grp in statistics)
            {
                var name = grp.Key;
                var stats = grp.Value;

                Console.WriteLine(
                    "{0,-40} {1,10} {2,10} {3,10} {4,15}",
                    TruncateString(name, 38),
                    stats.MinTime,
                    stats.MaxTime,
                    Math.Round(stats.AverageTime).ToString("G"),
                    stats.Count
                );
            }
        }

        private string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ?
                   value :
                   value.Substring(0, maxLength - 3) + "...";
        }
    }
}
