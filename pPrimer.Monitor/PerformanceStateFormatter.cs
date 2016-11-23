using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pPrimer.Business;

namespace pPrimer.Monitor
{
    using System.Runtime.InteropServices;

    public class PerformanceStateFormatter
    {
        private static readonly StringBuilder _result = new StringBuilder();

        public static string GetMessageForConcole(PerformanceState state)
        {
            _result.Clear();
            _result.Append($"CPU Process:{state.CpuTotalProcessUsagePercentage,3:###}%, ");
            _result.Append($"{string.Join(", ", state.CpuUsagePercentage.Select((cpu, i) => $"CPU{i}:{cpu,3:###}%"))}, ");
            _result.Append($"TotalMemory:{state.TotalMemoryBytes / 1024 / 1024,4:####} Mb, ");
            _result.Append($"WorkingSet:{state.WorkingSetBytes / 1024 / 1024,4:####} Mb, ");
            _result.Append($"Threads:{state.ThredCount}");

            return _result.ToString();
        }

        public static string GetMessageForStateLog(PerformanceState state)
        {
            _result.Clear();
            _result.Append($"{state.CpuTotalProcessUsagePercentage}, ");
            _result.Append($"{string.Join(", ", state.CpuUsagePercentage)}, ");
            _result.Append($"{state.TotalMemoryBytes}, ");
            _result.Append($"{state.WorkingSetBytes}, ");
            _result.Append($"{state.ThredCount}");

            return _result.ToString();
        }

        public static string GetMessageForStateLogHeader(PerformanceState state)
        {
            _result.Clear();
            _result.Append("CPU Total%, ");
            _result.Append($"{string.Join(", ", state.CpuUsagePercentage.Select((cpu,i) => $"CPU{ i}% "))}, ");
            _result.Append($"{nameof(state.TotalMemoryBytes)}, ");
            _result.Append($"{nameof(state.WorkingSetBytes)}, ");
            _result.Append($"{nameof(state.ThredCount)}");
            _result.Append("CPU Total%, ");

            return _result.ToString();
        }
    }
}
