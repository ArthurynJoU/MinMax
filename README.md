# MinMax

This is a **.NET Framework 4.8.1** console application that processes log files to determine the minimum, maximum, and average durations of various events. The application reads a log file, extracts timing information for each event, and outputs a console report summarizing these statistics.

---

## Project Structure

```
MinMax
├── Interfaces
│   ├── IApplication.cs
│   ├── IEventParser.cs
│   ├── IReportGenerator.cs
│   └── IStatisticsCalculator.cs
├── Models
│   ├── EventData.cs
│   └── EventStatistics.cs
├── Services
│   ├── ConsoleReportGenerator.cs
│   ├── EventParser.cs
│   └── StatisticsCalculator.cs
├── Application.cs
├── Program.cs
├── UnitTests.cs
└── MinMax.csproj
```

1. **Interfaces**  
   - `IApplication`: The entry point of the application’s core logic.  
   - `IEventParser`: Defines the method to read and parse the event log.  
   - `IReportGenerator`: Defines the method to generate a report of the calculated statistics.  
   - `IStatisticsCalculator`: Defines the method to calculate minimum, maximum, average, and count from the parsed data.

2. **Models**  
   - `EventData`: Holds the event name and its duration in milliseconds.  
   - `EventStatistics`: Holds min, max, average, and count of durations for a given event.

3. **Services**  
   - `ConsoleReportGenerator`: Prints the final statistics report to the console.  
   - `EventParser`: Reads and parses the log file to produce `EventData`.  
   - `StatisticsCalculator`: Computes min, max, average, and count from the list of `EventData` objects.

4. **Application.cs**  
   - The main application that ties together the parser, calculator, and report generator.

5. **Program.cs**  
   - The console entry point. It reads the command-line argument for the log file path and runs the application.

6. **UnitTests.cs**  
   - Contains manually implemented unit tests for `EventParser`, `StatisticsCalculator`, `ConsoleReportGenerator`, and `Application`.  
   - Tests are self-contained and run without any third-party frameworks, ensuring compliance with the "no third-party libraries" requirement.

---

## How to Build and Run

### Prerequisites
- Ensure **.NET Framework 4.8.1 Developer Pack** is installed on your machine.
- Use Visual Studio or a compatible IDE to work with .NET Framework projects.

### Building the Application
1. Open the solution in Visual Studio.
2. Build the solution:
   - **In Visual Studio**: Go to **Build** > **Build Solution**.
   - **Using MSBuild**: Open a terminal and run:
     ```bash
     msbuild MinMax.csproj
     ```

### Running the Application
Since this is a .NET Framework project, it generates an `.exe` file. Run the `.exe` file directly.

1. Locate the compiled executable:
   ```
   bin\Debug\MinMax.exe
   ```
2. Run the application from the terminal or command prompt:
   ```bash
   MinMax.exe <path_to_log_file>
   ```
   Replace `<path_to_log_file>` with the full path to your log file.

---

## Running Unit Tests

Unit tests are implemented in `UnitTests.cs` and run automatically when the application starts.

1. Simply run the compiled `.exe`:
   ```bash
   MinMax.exe
   ```
2. The tests will execute first, followed by the main program logic. Test results will be displayed in the console output.

Example test output:
```
Starting Unit Tests...

Running TestEventParser...
[PASS] TestEventParser: Empty log
[PASS] TestEventParser: Valid log lines
[PASS] TestEventParser: Invalid log lines
TestEventParser Completed.

Running TestStatisticsCalculator...
[PASS] TestStatisticsCalculator: Contains EventA
[PASS] TestStatisticsCalculator: MinTime for EventA
[PASS] TestStatisticsCalculator: MaxTime for EventA
[PASS] TestStatisticsCalculator: AverageTime for EventA
[PASS] TestStatisticsCalculator: Count for EventA
[PASS] TestStatisticsCalculator: Contains EventB
[PASS] TestStatisticsCalculator: MinTime for EventB
[PASS] TestStatisticsCalculator: MaxTime for EventB
[PASS] TestStatisticsCalculator: AverageTime for EventB
[PASS] TestStatisticsCalculator: Count for EventB
TestStatisticsCalculator Completed.

Running TestConsoleReportGenerator...
[PASS] TestConsoleReportGenerator: Contains EventA
[PASS] TestConsoleReportGenerator: Contains Average for EventA
[PASS] TestConsoleReportGenerator: Contains EventB
[PASS] TestConsoleReportGenerator: Contains Average for EventB
TestConsoleReportGenerator Completed.

Running TestApplication...
[PASS] TestApplication: ReportGenerator was called
TestApplication Completed.

Unit Tests Completed.
Passed: 20, Failed: 0
```

---

## How It Works

1. **Event Parsing**  
   - The `EventParser` reads each line of the log file.  
   - It looks for lines containing `"[Performance]"` and the substring `"... has been processed in ... ms"`.  
   - It extracts the event name and the duration (milliseconds).

2. **Statistics Calculation**  
   - The `StatisticsCalculator` groups the parsed event data by event name.  
   - For each group, it computes the minimum duration, maximum duration, average duration, and total count.

3. **Reporting**  
   - The `ConsoleReportGenerator` displays the statistics in a tabular format.

---

## Example

Given a log file with the following lines:
```
[Performance] EventA with Tid 1 has been processed in 100 ms
[Performance] EventB with Tid 2 has been processed in 200 ms
[Performance] EventA with Tid 3 has been processed in 150 ms
```

The console output will show:
```
Event                                           Min        Max    Average           Count
EventA                                          100        150    125               2
EventB                                          200        200    200               1
```

---
