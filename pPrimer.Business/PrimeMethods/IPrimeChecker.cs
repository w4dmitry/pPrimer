using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pPrimer.Business
{
    public interface IPrimeChecker
    {
        bool IsPrime(int number);
        string DisplayName { get; }
    }
}
