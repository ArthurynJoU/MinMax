using MinMax.Interfaces;
using System;
using System.Linq;

namespace MinMax
{
    public class Application : IApplication
    {
        private readonly IEventParser parser;
        private readonly IStatisticsCalculator calculator;
        private readonly IReportGenerator reportGenerator;

        public Application(IEventParser parser,
                           IStatisticsCalculator calculator,
                           IReportGenerator reportGenerator)
        {
            this.parser = parser;
            this.calculator = calculator;
            this.reportGenerator = reportGenerator;
        }

        public void Run(string logFilePath)
        {
            if (string.IsNullOrWhiteSpace(logFilePath))
            {
                Console.WriteLine("File path must not be empty.");
                return;
            }
            
            var eventData = parser.ParseEvents(logFilePath);
            if (!eventData.Any())
            {
                Console.WriteLine("No data found to process.");
                return;
            }

            var statistics = calculator.CalculateStatistics(eventData);
            reportGenerator.GenerateReport(statistics);
        }
    }

}
