using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pPrimer.Business.Properties;

namespace pPrimer.Business
{
    public class PrimeRunnerContainer
    {
        public PrimeRunnerContainer(string name, string displayName, Type runnerType)
        {
            if (!typeof(IRunner).IsAssignableFrom(runnerType))
                throw new ArgumentException(string.Format(Strings.TypeMustImplement, "Runner", nameof(IRunner)));

            Name = name;
            DisplayName = displayName;
            Type = runnerType;
        }
        public string Name { get; }
        public string DisplayName { get; }
        public Type Type { get; }
    }
}
