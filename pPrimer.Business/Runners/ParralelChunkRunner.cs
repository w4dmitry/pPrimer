using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using pPrimer.Business.Properties;

namespace pPrimer.Business
{
    public class ParralelChunkRunner : IRunner
    {
        private readonly IPrimeChecker _checker;

        public ParralelChunkRunner()
        {
        }

        public ParralelChunkRunner(IPrimeChecker checker)
        {
            if (checker == null)
                throw new ArgumentException(string.Format(Strings.ArgumentCannotBeNull, "checker"));

            _checker = checker;
        }
        /// <summary>
        /// Executes structural data parallelism, chunk partitioning
        /// </summary>
        /// <param name="topLimit"></param>
        /// <returns>Unordered list of prime numbers</returns>
        public IEnumerable<int> GetAllNumbers(int topLimit)
        {
            if (topLimit < PrimeNumber.FIRST_PRIME_NUMBER)
                throw new ArgumentException(string.Format(Strings.ArgumentShouldBeNoLessThan, PrimeNumber.FIRST_PRIME_NUMBER));

            var partitioner = Partitioner.Create(PrimeNumber.FIRST_PRIME_NUMBER, topLimit);
            var lockObject = new object();
            var result = new List<int>();

            Parallel.ForEach(partitioner,
                    () => new List<int>(),
                    (range, state, localList) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                            if( _checker.IsPrime(i) )
                                localList.Add(i);

                        return localList;
                    },
                    finalResult =>
                    {
                        lock (lockObject)
                            result.AddRange(finalResult);
                    });

            return result;
        }

        public string DisplayName => Strings.ParralelChunk;
    }
}
