using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pPrimer.Business.Properties;

namespace pPrimer.Business
{
    public class PrimeMethodContainer
    {
        public PrimeMethodContainer(string name, string displayName, Type methodType)
        {
            if (!typeof(IPrimeChecker).IsAssignableFrom(methodType))
                throw new ArgumentException(string.Format(Strings.TypeMustImplement, "Method", nameof(IPrimeChecker)));

            Name = name;
            DisplayName = displayName;
            Type = methodType;
        }
        public string Name { get; }
        public string DisplayName { get; }
        public Type Type { get; }
    }
}
