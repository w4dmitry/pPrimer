using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pPrimer.Business;

namespace pPrimer.Web.Models
{
    public class PerformanceStateListViewModel
    {
        public PerformanceStateListViewModel(IEnumerable<PerformanceState> states)
        {
            States = new List<PerformanceStateViewModel>();

            foreach (var state in states)
                States.Add(new PerformanceStateViewModel(state));
        }

        public IList<PerformanceStateViewModel> States { get; set; }
    }

    public class PerformanceStateViewModel
    {
        public PerformanceStateViewModel(PerformanceState state)
        {
            CpuUsagePercentage = state.CpuUsagePercentage;
            CpuTotalProcessUsagePercentage = state.CpuTotalProcessUsagePercentage;
            WorkingSetBytes = state.WorkingSetBytes;
            TotalMemoryBytes = state.TotalMemoryBytes;
            TotalCpus = state.TotalCpus;
            ThredCount = state.ThredCount;
            TimeStamp = state.TimeStamp;
        }

        public int TotalCpus { get; set; }
        public IList<float> CpuUsagePercentage { get; set; }
        public float CpuTotalProcessUsagePercentage { get; set; }
        public float WorkingSetBytes { get; set; }
        public long TotalMemoryBytes { get; set; }
        public float ThredCount { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}

