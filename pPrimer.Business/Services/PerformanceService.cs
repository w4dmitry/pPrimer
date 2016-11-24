using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace pPrimer.Business.Services
{
    using System.Collections.Concurrent;
    using System.Threading;

    using log4net;

    using pPrimer.Business.Tools;

    public class PerformanceService : IPerformanceService, IDisposable
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

        private readonly TaskCompletionSource<bool> _queryStateTcs;
        private readonly TimeSpan _queryTaskPeriod = TimeSpan.FromSeconds(1);
        private readonly int _limitQueryQueue = 60; // 60 seconds or 60 samples
        private readonly ConcurrentQueue<PerformanceState> _queryQueue = new ConcurrentQueue<PerformanceState>();

        private readonly ILog _logger;

        public PerformanceService(ILog logger)
        {
            if (logger == null)
                throw new ArgumentException(string.Format(Strings.ArgumentCannotBeNull, "logger"));
            _logger = logger;

            var instanceName = Process.GetCurrentProcess().ProcessName;
            _cpuCount = Environment.ProcessorCount;
            _cpuUsagePercentage = new List<PerformanceCounter>();

            _cpuTotalProcessUsagePercentage = new PerformanceCounter(ProcesCategory, ProcessCategoryCpuTime, instanceName);

            for (int cpuNumber = 0; cpuNumber < _cpuCount; cpuNumber++)
                _cpuUsagePercentage.Add(new PerformanceCounter(ProcessorCategory, ProcessCategoryCpuTime, cpuNumber.ToString()));

            _workingSet = new PerformanceCounter(ProcesCategory, ProcessCategoryWorkingSet, instanceName);

            _threadCountTotalProcess = new PerformanceCounter(ProcesCategory, ProcessCategoryThreadCount, ThreadCountParam);

            _queryStateTcs = ((Action)QueryState).ToPeriodicTask<bool>(_queryTaskPeriod, _queryTaskPeriod);
        }

        private void QueryState()
        {
            // Generate new state
            var newState = new PerformanceState
                               {
                                   CpuTotalProcessUsagePercentage = GetCpuTotalProcessUsagePercentageValue(),
                                   CpuUsagePercentage = GetCpuUsagePercentageValues(),
                                   WorkingSetBytes = GetWorkingSetBytesValue(),
                                   TotalMemoryBytes = GetTotalMemoryBytesValue(),
                                   ThredCount = GetThredCountValue(),
                                   TotalCpus = _cpuCount,
                                   TimeStamp = DateTime.UtcNow
                               };

            _queryQueue.Enqueue(newState);

            // Cycle buffer behaviour
            // Calling count is O(n), but performance is not important here
            while (_queryQueue.Count > _limitQueryQueue)
                GetCounters();
        }

        private PerformanceState GetCounters()
        {
            PerformanceState pCounter;
            _queryQueue.TryDequeue(out pCounter);

            return pCounter;
        }

        public async Task<IEnumerable<PerformanceState>> GetState()
        {
            return await Task.Run(
                       () =>
                           {
                               var list = new List<PerformanceState>();
                               var state = GetCounters();
                               if(state != null)
                                   list.Add(state);

                               return list.Count > 0 ? list : null;
                           });
        }

        private float GetThredCountValue()
        {
            return _threadCountTotalProcess.NextValue();
        }

        private static long GetTotalMemoryBytesValue()
        {
            return GC.GetTotalMemory(false);
        }

        private float GetWorkingSetBytesValue()
        {
            return _workingSet.NextValue();
        }

        private List<float> GetCpuUsagePercentageValues()
        {
            return _cpuUsagePercentage.Select(cpuUsage => cpuUsage.NextValue()).ToList();
        }

        private float GetCpuTotalProcessUsagePercentageValue()
        {
            return _cpuTotalProcessUsagePercentage.NextValue();
        }

        public void Dispose()
        {
            // Dispose called by Unity for ContainerControlledLifetimeManager
            _queryStateTcs.TrySetCanceled();

            if (_queryStateTcs.Task.IsFaulted)
            {
                _logger.Error("Performance query task faulted!");
            }
        }
    }
}
