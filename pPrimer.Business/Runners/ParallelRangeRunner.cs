using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pPrimer.Business.Properties;

namespace pPrimer.Business
{
    public class ParallelRangeRunner : IRunner
    {
        private readonly IPrimeChecker _checker;

        public ParallelRangeRunner()
        {
        }

        public ParallelRangeRunner(IPrimeChecker checker)
        {
            if(checker == null)
                throw new ArgumentException(string.Format(Strings.ArgumentCannotBeNull, "checker"));

            _checker = checker;
        }
        /// <summary>
        /// Executes structural data parallelism, range partitioning
        /// </summary>
        /// <param name="topLimit"></param>
        /// <returns>Unordered list of prime numbers</returns>
        public IEnumerable<int> GetAllNumbers(int topLimit)
        {
            if (topLimit  < PrimeNumber.FIRST_PRIME_NUMBER)
                throw new ArgumentException(string.Format(Strings.ArgumentShouldBeNoLessThan, PrimeNumber.FIRST_PRIME_NUMBER));

            return ParallelEnumerable.Range(PrimeNumber.FIRST_PRIME_NUMBER, topLimit - PrimeNumber.FIRST_PRIME_NUMBER + 1)
                                              .Where(n => _checker.IsPrime(n));
        }

        public string DisplayName => Strings.ParralelRange;
    }
}
