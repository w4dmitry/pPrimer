using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using pPrimer.Business;

namespace pPrimer.Web.Models
{
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
        }

        public int TotalCpus { get; set; }
        public IList<float> CpuUsagePercentage { get; set; }
        public float CpuTotalProcessUsagePercentage { get; set; }
        public float WorkingSetBytes { get; set; }
        public long TotalMemoryBytes { get; set; }
        public float ThredCount { get; set; }
    }
}

