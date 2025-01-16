using MinMax.Interfaces;
using MinMax.Services;
using System;

namespace MinMax
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitTests.RunAllTests();

            if (args.Length == 0)
            {
                Console.WriteLine("Enter the path to the file.");
                return;
            }

            string logFilePath = args[0];

            IEventParser parser = new EventParser();
            IStatisticsCalculator calculator = new StatisticsCalculator();
            IReportGenerator reportGenerator = new ConsoleReportGenerator();
            IApplication app = new Application(parser, calculator, reportGenerator);

            app.Run(logFilePath);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

}
