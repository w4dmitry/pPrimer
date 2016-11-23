using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace pPrimer.Business.Tests.TypeResolver
{
    using pPrimer.Business.Runners;
    using pPrimer.Business.TypeResolver;

    [TestClass]
    public class PrimeFacilitiesResolverTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfMethodTypeIsInvalid()
        {
            var anyWrongMethodType = typeof(DateTime);
            var anyProperRunnerType = typeof(SequencialRunner);
            var resolver = new PrimeFacilitiesResolver();

            resolver.GetRunner(anyWrongMethodType, anyProperRunnerType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfRunnerTypeIsInvalid()
        {
            var anyWrongRunnerType = typeof(DateTime);
            var anyProperMethodType = typeof(PrimeSquareBasedMethod);
            var resolver = new PrimeFacilitiesResolver();

            resolver.GetRunner(anyProperMethodType, anyWrongRunnerType);
        }

        [TestMethod]
        public void ShouldGetProperRunnerInstance()
        {
            var anyProperMethodType = typeof(PrimeSquareBasedMethod);
            var anyProperRunnerType = typeof(SequencialRunner);
            var resolver = new PrimeFacilitiesResolver();

            var result = resolver.GetRunner(anyProperMethodType, anyProperRunnerType);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IRunner));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfMethodTypeIsNull()
        {
            var anyWrongMethodType = (Type)null;
            var anyProperRunnerType = typeof(SequencialRunner);
            var resolver = new PrimeFacilitiesResolver();

            resolver.GetRunner(anyWrongMethodType, anyProperRunnerType);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfRunnerTypeIsNull()
        {
            var anyProperMethodType = typeof(PrimeSquareBasedMethod);
            var anyWrongRunnerType = (Type)null;

            var resolver = new PrimeFacilitiesResolver();

            resolver.GetRunner(anyProperMethodType, anyWrongRunnerType);
        }

        [TestMethod]
        public void ShouldGetProperPerformanceRunnerInstance()
        {
            var anyProperMethodType = typeof(PrimeSquareBasedMethod);
            var anyProperRunnerType = typeof(SequencialRunner);
            var resolver = new PrimeFacilitiesResolver();

            var result = resolver.GetPerformanceRunner(anyProperMethodType, anyProperRunnerType);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(PerformanceRunnerWrapper));
        }
    }
}
