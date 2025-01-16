using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MinMax.Interfaces;
using MinMax.Models;
using MinMax.Services;

namespace MinMax
{
    public static class UnitTests
    {
        private static int passedTests = 0;
        private static int failedTests = 0;

        public static void RunAllTests()
        {
            Console.WriteLine("Starting Unit Tests...\n");

            TestEventParser();
            TestStatisticsCalculator();
            TestConsoleReportGenerator();
            TestApplication();

            Console.WriteLine("\nUnit Tests Completed.");
            Console.WriteLine($"Passed: {passedTests}, Failed: {failedTests}");
        }

        private static void TestEventParser()
        {
            Console.WriteLine("Running TestEventParser...");

            var parser = new EventParser();

            // Test: Empty log
            var tempFilePath = Path.GetTempFileName();
            try
            {
                File.WriteAllText(tempFilePath, "");
                var events = parser.ParseEvents(tempFilePath);
                Assert(events.Count() == 0, "TestEventParser: Empty log");
            }
            finally
            {
                File.Delete(tempFilePath);
            }

            // Test: Valid log lines
            var validLog = new List<string>
            {
                "[Performance] EventA with Tid 1 has been processed in 100 ms",
                "[Performance] EventB with Tid 2 has been processed in 200 ms"
            };
            tempFilePath = Path.GetTempFileName();
            try
            {
                File.WriteAllLines(tempFilePath, validLog);
                var events = parser.ParseEvents(tempFilePath);
                Assert(events.Count() == 2, "TestEventParser: Valid log lines");
            }
            finally
            {
                File.Delete(tempFilePath);
            }

            // Test: Invalid log lines
            var invalidLog = new List<string>
            {
                "Random log line with no data"
            };
            tempFilePath = Path.GetTempFileName();
            try
            {
                File.WriteAllLines(tempFilePath, invalidLog);
                var events = parser.ParseEvents(tempFilePath);
                Assert(events.Count() == 0, "TestEventParser: Invalid log lines");
            }
            finally
            {
                File.Delete(tempFilePath);
            }

            Console.WriteLine("TestEventParser Completed.");
        }

        private static void TestStatisticsCalculator()
        {
            Console.WriteLine("Running TestStatisticsCalculator...");

            var calculator = new StatisticsCalculator();

            var input = new List<EventData>
            {
                new EventData { Name = "EventA", Duration = 100 },
                new EventData { Name = "EventA", Duration = 200 },
                new EventData { Name = "EventB", Duration = 300 }
            };

            var stats = calculator.CalculateStatistics(input);

            Assert(stats.ContainsKey("EventA"), "TestStatisticsCalculator: Contains EventA");
            Assert(stats["EventA"].MinTime == 100, "TestStatisticsCalculator: MinTime for EventA");
            Assert(stats["EventA"].MaxTime == 200, "TestStatisticsCalculator: MaxTime for EventA");
            Assert(stats["EventA"].AverageTime == 150, "TestStatisticsCalculator: AverageTime for EventA");
            Assert(stats["EventA"].Count == 2, "TestStatisticsCalculator: Count for EventA");

            Assert(stats.ContainsKey("EventB"), "TestStatisticsCalculator: Contains EventB");
            Assert(stats["EventB"].MinTime == 300, "TestStatisticsCalculator: MinTime for EventB");
            Assert(stats["EventB"].MaxTime == 300, "TestStatisticsCalculator: MaxTime for EventB");
            Assert(stats["EventB"].AverageTime == 300, "TestStatisticsCalculator: AverageTime for EventB");
            Assert(stats["EventB"].Count == 1, "TestStatisticsCalculator: Count for EventB");

            Console.WriteLine("TestStatisticsCalculator Completed.");
        }

        private static void TestConsoleReportGenerator()
        {
            Console.WriteLine("Running TestConsoleReportGenerator...");

            var reportGenerator = new ConsoleReportGenerator();
            var stats = new Dictionary<string, EventStatistics>
            {
                {
                    "EventA", new EventStatistics
                    {
                        MinTime = 100,
                        MaxTime = 200,
                        AverageTime = 150.0,
                        Count = 2
                    }
                },
            {
                "EventB", new EventStatistics
                {   
                    MinTime = 300,
                    MaxTime = 300,
                    AverageTime = 300.0,
                    Count = 1
                }
            }
        };

            var originalOut = Console.Out;
            using (var sw = new StringWriter())
            {
                try
                {
                    Console.SetOut(sw);

                    reportGenerator.GenerateReport(stats);

                    var output = sw.ToString();
                    Assert(output.Contains("EventA"), "TestConsoleReportGenerator: Contains EventA");
                    Assert(output.Contains("150"), "TestConsoleReportGenerator: Contains Average for EventA");
                    Assert(output.Contains("EventB"), "TestConsoleReportGenerator: Contains EventB");
                    Assert(output.Contains("300"), "TestConsoleReportGenerator: Contains Average for EventB");
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
            }

            Console.WriteLine("TestConsoleReportGenerator Completed.");
        }


        private static void TestApplication()
        {
            Console.WriteLine("Running TestApplication...");

            var fakeParser = new FakeEventParser(new List<EventData>
            {
                new EventData { Name = "EventA", Duration = 100 },
                new EventData { Name = "EventB", Duration = 200 }
            });

            var fakeCalculator = new FakeStatisticsCalculator(new Dictionary<string, EventStatistics>
            {
                { "EventA", new EventStatistics { MinTime = 100, MaxTime = 100, AverageTime = 100, Count = 1 } },
                { "EventB", new EventStatistics { MinTime = 200, MaxTime = 200, AverageTime = 200, Count = 1 } }
            });

            var fakeReportGenerator = new FakeReportGenerator();

            var app = new Application(fakeParser, fakeCalculator, fakeReportGenerator);
            app.Run("dummyPath");

            Assert(fakeReportGenerator.WasCalled, "TestApplication: ReportGenerator was called");

            Console.WriteLine("TestApplication Completed.");
        }

        private static void Assert(bool condition, string testName)
        {
            if (condition)
            {
                passedTests++;
                Console.WriteLine($"[PASS] {testName}");
            }
            else
            {
                failedTests++;
                Console.WriteLine($"[FAIL] {testName}");
            }
        }

        private class FakeEventParser : IEventParser
        {
            private readonly IEnumerable<EventData> _data;

            public FakeEventParser(IEnumerable<EventData> data)
            {
                _data = data;
            }

            public IEnumerable<EventData> ParseEvents(string logFilePath) => _data;
        }

        private class FakeStatisticsCalculator : IStatisticsCalculator
        {
            private readonly IDictionary<string, EventStatistics> _stats;

            public FakeStatisticsCalculator(IDictionary<string, EventStatistics> stats)
            {
                _stats = stats;
            }

            public IDictionary<string, EventStatistics> CalculateStatistics(IEnumerable<EventData> events) => _stats;
        }

        private class FakeReportGenerator : IReportGenerator
        {
            public bool WasCalled { get; private set; }

            public void GenerateReport(IDictionary<string, EventStatistics> statistics)
            {
                WasCalled = true;
            }
        }
    }
}
