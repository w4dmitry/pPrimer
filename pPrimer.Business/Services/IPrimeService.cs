using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pPrimer.Business.Services
{
    public interface IPrimeService
    {
        IEnumerable<PrimeMethodSet> MethodsSets { get; }
        string StartCalculation(MethodIdNumberPairContainer methodIdNumberPairContainer);

        PrimeCalculationStatus GetStatus(string sid);
    }
}
