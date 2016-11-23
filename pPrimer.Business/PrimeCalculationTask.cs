using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pPrimer.Business
{
    public class PrimeCalculationTask
    {
        public Task<PrimeCalculationResult> Task { get; set; }

        public PrimeCalculationResult Result => Task.Result;

        public PrimeMethodSet MethodSet { get; set; }

        public bool HasErrors { get; set; }

        public IEnumerable<string> Errors { get; set; }

        public DateTime StartTime { get; set; }
    }
}
