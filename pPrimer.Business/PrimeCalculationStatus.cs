using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pPrimer.Business
{
    public class PrimeCalculationStatus
    {
        public bool IsCompleted { get; set; }

        public IEnumerable<PrimeCalculationTask> Tasks { get; set; }
    }
}
