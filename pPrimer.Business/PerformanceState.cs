using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pPrimer.Business
{
    public class PerformanceState
    {
        public int TotalCpus { get; set; }
        public IList<float> CpuUsagePercentage { get; set; }
        public float CpuTotalProcessUsagePercentage { get; set; }
        public float WorkingSetBytes { get; set; }
        public long TotalMemoryBytes { get; set; }
        public float ThredCount { get; set; }
    }
}
