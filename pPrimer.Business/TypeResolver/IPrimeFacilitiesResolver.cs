using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pPrimer.Business.Runners;

namespace pPrimer.Business.TypeResolver
{
    public interface IPrimeFacilitiesResolver
    {
        IEnumerable<PrimeMethodSet> MethodSets { get; }

        IRunner GetRunner(Type methodType, Type runnerType);

        PerformanceRunnerWrapper GetPerformanceRunner(Type methodType, Type runnerType);
    }
}
