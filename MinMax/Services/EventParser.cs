using MinMax.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace MinMax.Services
{
    public class EventParser : IEventParser
    {
        private const string startMarker = "[Event] Processing "    ;
        private const string endMarker   = "[Performance] "         ;
        private const string endSuffix   = " has been processed in ";
        private const string durationUnit= "ms"                     ;

        public IEnumerable<EventData> ParseEvents(string logFilePath)
        {
            var result = new List<EventData>();
            try
            {
                var lines = File.ReadAllLines(logFilePath);

                foreach (var line in lines)
                {
                    if (line.Contains(endMarker) && 
                        line.Contains("has been processed in"))
                    {
                        var performanceIndex = line.IndexOf(
                            endMarker, StringComparison.OrdinalIgnoreCase);
                        if (performanceIndex < 0) continue;

                        var eventWithoutMarker = line.Substring(
                            performanceIndex + endMarker.Length);

                        var tidIndex = eventWithoutMarker.IndexOf(
                            " with Tid", StringComparison.OrdinalIgnoreCase);
                        if (tidIndex < 0) continue;

                        string name = eventWithoutMarker
                                        .Substring(0, tidIndex)
                                        .Trim();

                        var endSuffixIndex = eventWithoutMarker.IndexOf(
                            endSuffix, StringComparison.OrdinalIgnoreCase);
                        if (endSuffixIndex < 0) continue;

                        var durationWithoutMarker = eventWithoutMarker.Substring(
                            endSuffixIndex + endSuffix.Length);

                        durationWithoutMarker = durationWithoutMarker
                            .Replace(durationUnit + ".", "")
                            .Replace(durationUnit, "")
                            .Trim();

                        if (int.TryParse(durationWithoutMarker, out int duration))
                        {
                            result.Add(new EventData
                            {
                                Name = name,
                                Duration = duration
                            });
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Error: The file at path ‘{logFilePath}’ was not found.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Error: Access to file ‘{logFilePath}’ was denied.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading a file: {ex.Message}");
            }

            return result;
        }
    }

}
