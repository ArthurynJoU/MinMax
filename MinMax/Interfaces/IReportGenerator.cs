using MinMax.Models;
using System.Collections.Generic;

namespace MinMax.Interfaces
{
    public interface IReportGenerator
    {
        void GenerateReport(IDictionary<string, EventStatistics> statistics);
    }

}
