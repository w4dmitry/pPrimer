using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pPrimer.Business.Properties;

namespace pPrimer.Business
{
    public class PrimeSquareBasedMethod : IPrimeChecker
    {
        public PrimeSquareBasedMethod()
        {
        }

        public bool IsPrime(int number)
        {
            if ((number & 1) == 0)
            {
                return (number == 2);
            }

            for (int i = 3; (i * i) <= number; i += 2)
            {
                if ((number % i) == 0)
                {
                    return false;
                }
            }

            return number != 1;
        }

        public string DisplayName => Strings.PrimeSquareBasedMethod;
    }
}
