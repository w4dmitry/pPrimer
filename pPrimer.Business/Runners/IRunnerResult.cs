using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pPrimer.Business.Runners
{
    public interface IRunnerResult
    {
       PrimeCalculationResult Result { get; }
    }
}
