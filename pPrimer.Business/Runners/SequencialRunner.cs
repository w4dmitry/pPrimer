using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pPrimer.Business.Properties;

namespace pPrimer.Business
{
    public class SequencialRunner : IRunner
    {
        private readonly IPrimeChecker _checker;

        public SequencialRunner()
        {
        }

        public SequencialRunner(IPrimeChecker checker)
        {
            if (checker == null)
                throw new ArgumentException(string.Format(Strings.ArgumentCannotBeNull, "checker"));

            _checker = checker;
        }
        public IEnumerable<int> GetAllNumbers(int topLimit)
        {
            if (topLimit < PrimeNumber.FIRST_PRIME_NUMBER)
                throw new ArgumentException(string.Format(Strings.ArgumentShouldBeNoLessThan, PrimeNumber.FIRST_PRIME_NUMBER));

            return Enumerable.Range(PrimeNumber.FIRST_PRIME_NUMBER, topLimit - PrimeNumber.FIRST_PRIME_NUMBER + 1)
                .Where(n => _checker.IsPrime(n));
        }

        public string DisplayName => Strings.Sequential;
    }
}
