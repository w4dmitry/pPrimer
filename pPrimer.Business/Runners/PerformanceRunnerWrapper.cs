using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pPrimer.Business.Properties;

namespace pPrimer.Business.Runners
{
    public class PerformanceRunnerWrapper: IRunner, IRunnerResult
    {
        private readonly IRunner _runner;

        private PrimeCalculationResult _result;

        public PerformanceRunnerWrapper(IRunner runner)
        {
            if (runner == null)
                throw new ArgumentException(string.Format(Strings.ArgumentCannotBeNull, "runner"));

            _runner = runner;
        }

        public PrimeCalculationResult Result => _result;

        public IEnumerable<int> GetAllNumbers(int topLimit)
        {
            _result = new PrimeCalculationResult();

            _result.StartTime = DateTime.UtcNow;

            _result.Primes = _runner.GetAllNumbers(topLimit).ToList();

            _result.EndTime = DateTime.UtcNow;

            return _result.Primes;
        }

        public string DisplayName => _runner.DisplayName;
    }
}
