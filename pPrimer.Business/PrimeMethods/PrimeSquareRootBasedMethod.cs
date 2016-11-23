using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pPrimer.Business.Properties;

namespace pPrimer.Business
{
    public class PrimeSquareRootBasedMethod : IPrimeChecker
    {
        public PrimeSquareRootBasedMethod()
        {
        }

        public bool IsPrime(int number)
        {
            if ((number & 1) == 0)
            {
                return (number == 2);
            }
            else
            {
                if (number == 1)
                    return false;
            }

            return Enumerable.Range(2, (int)Math.Sqrt(number) - 1).All(i => number % i > 0);
        }

        public string DisplayName => Strings.PrimeSquareRootBasedMethod;
    }
}
