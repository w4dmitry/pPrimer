using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace pPrimer.Business.Services
{
    public class PerformanceService : IPerformanceService
    {
        private readonly PerformanceCounter _cpuTotalUsagePercentage;
        private readonly List<PerformanceCounter> _cpuUsagePercentage;
        private readonly PerformanceCounter _cpuTotalProcessUsagePercentage;
        private readonly PerformanceCounter _workingSet;
        private readonly PerformanceCounter _threadCountTotalProcess;

        private readonly string ProcesCategory = "Process";
        private readonly string ProcessorCategory = "Processor";

        private readonly string ProcessCategoryCpuTime = "% Processor Time";
        private readonly string ProcessCategoryWorkingSet = "Working Set";
        private readonly string ProcessCategoryThreadCount = "Thread Count";

        private readonly string ThreadCountParam = "_Total";

        private readonly int _cpuCount;

        public PerformanceService()
        {
            var instanceName = Process.GetCurrentProcess().ProcessName;
            _cpuCount = Environment.ProcessorCount;
            _cpuUsagePercentage = new List<PerformanceCounter>();

            _cpuTotalProcessUsagePercentage = new PerformanceCounter(ProcesCategory, ProcessCategoryCpuTime, instanceName);

            for (int cpuNumber = 0; cpuNumber < _cpuCount; cpuNumber++)
                _cpuUsagePercentage.Add(new PerformanceCounter(ProcessorCategory, ProcessCategoryCpuTime, cpuNumber.ToString()));

            _workingSet = new PerformanceCounter(ProcesCategory, ProcessCategoryWorkingSet, instanceName);

            _threadCountTotalProcess = new PerformanceCounter(ProcesCategory, ProcessCategoryThreadCount, ThreadCountParam);
        }

        public async Task<PerformanceState> GetState()
        {

            return await Task.Run(() =>
                       {
                           return new PerformanceState
                                      {
                                          CpuTotalProcessUsagePercentage = _cpuTotalProcessUsagePercentage.NextValue(),
                                          CpuUsagePercentage =_cpuUsagePercentage.Select(cpuUsage => cpuUsage.NextValue()).ToList(),
                                          WorkingSetBytes = _workingSet.NextValue(),
                                          TotalMemoryBytes = GC.GetTotalMemory(false),
                                          ThredCount = _threadCountTotalProcess.NextValue(),
                                          TotalCpus = _cpuCount
                                      };
                       });
        }
    }
}
