using MinMax.Models;
using System.Collections.Generic;

namespace MinMax
{
    public interface IEventParser
    {
        /// <summary>
        /// Reads data from the log and returns a collection of EventData objects,
        /// where the event name and time (ms) are stored.
        /// </summary>
        IEnumerable<EventData> ParseEvents(string logFilePath);
    }
}
