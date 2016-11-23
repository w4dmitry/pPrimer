using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pPrimer.Business.Properties;

namespace pPrimer.Business
{
    public class PrimeMethodSet
    {
        private readonly int DEFAULT_TOP_NUMBER = 100;

        private static int _idCounter = 0;

        private readonly PrimeMethodContainer _method;

        private readonly PrimeRunnerContainer _runner;

        public PrimeMethodSet(PrimeMethodContainer method, PrimeRunnerContainer runner)
        {
            if(method == null)
                throw new ArgumentException(string.Format(Strings.ArgumentCannotBeNull, "method"));

            if (runner == null)
                throw new ArgumentException(string.Format(Strings.ArgumentCannotBeNull, "runner"));

            _method = method;
            _runner = runner;
            Id = _idCounter++;
            TopNumber = DEFAULT_TOP_NUMBER;
        }

        public int TopNumber { get; set; }
        public virtual int Id { get; }

        public string MethodName => _method.Name;

        public string MethodDisplayName => _method.DisplayName;

        public string RunnerName => _runner.Name;

        public string RunnerDisplayName => _runner.DisplayName;

        public Type MethodType => _method.Type;

        public Type RunnerType => _runner.Type;

        public string DisplayName => $"{MethodDisplayName} / {RunnerDisplayName}";
    }
}
