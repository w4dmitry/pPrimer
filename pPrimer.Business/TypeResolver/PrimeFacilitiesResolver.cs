using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using pPrimer.Business.Runners;
using pPrimer.Business.Properties;

namespace pPrimer.Business.TypeResolver
{
    public class PrimeFacilitiesResolver : IPrimeFacilitiesResolver
    {
        private static readonly string PERFORMANCE_RUNNER_PREFIX = "Performance";

        private Lazy<IEnumerable<PrimeMethodSet>> _methodSets = new Lazy<IEnumerable<PrimeMethodSet>>(GetMethodSets);

        public IEnumerable<PrimeMethodSet> MethodSets => _methodSets.Value;

        public IRunner GetRunner(Type methodType, Type runnerType)
        {
            if(!typeof(IPrimeChecker).IsAssignableFrom(methodType))
                throw new ArgumentException(string.Format(Strings.TypeMustImplement, "Method", nameof(IPrimeChecker)));

            if (!typeof(IRunner).IsAssignableFrom(runnerType))
                throw new ArgumentException(string.Format(Strings.TypeMustImplement, "Runner", nameof(IRunner)));

            var methodToRun = (IPrimeChecker)Activator.CreateInstance(methodType);
            var runner = (IRunner)Activator.CreateInstance(runnerType, methodToRun);
            return runner;
        }

        public PerformanceRunnerWrapper GetPerformanceRunner(Type methodType, Type runnerType)
        {
            var runner = GetRunner(methodType, runnerType);
            var performanceRunner = new PerformanceRunnerWrapper(runner);
            return performanceRunner;
        }

        private static IEnumerable<PrimeMethodSet> GetMethodSets()
        {
            var methodTypes = GetPrimeTypes<IPrimeChecker>();
            var methodInstances = GetPrimeInstances<IPrimeChecker>(methodTypes);

            var runnerTypes = GetPrimeTypes<IRunner>();
            var runnerInstances = GetPrimeInstances<IRunner>(runnerTypes);

            var methods = from method in methodInstances
                          from runner in runnerInstances
                          select
                          new PrimeMethodSet(
                              new PrimeMethodContainer(method.GetType().Name, method.DisplayName, method.GetType()),
                              new PrimeRunnerContainer(runner.GetType().Name, runner.DisplayName, runner.GetType()));

            return methods.ToList();
        }

        private static IEnumerable<T> GetPrimeInstances<T>(IEnumerable<Type> types)
        {
            return types.Select(type => (T)Activator.CreateInstance(type));
        }

        private static IEnumerable<Type> GetPrimeTypes<T>()
        {
            var result = new List<Type>();
            var methodType = typeof(T);
            var assembly = Assembly.GetAssembly(typeof(PrimeFacilitiesResolver));
            {
                if (assembly != null)
                {
                    var types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                            continue;

                        if (type.GetInterface(methodType.FullName) != null && !type.Name.StartsWith(PERFORMANCE_RUNNER_PREFIX))
                            result.Add(type);
                    }
                }
            }

            return result;
        }
    }
}
