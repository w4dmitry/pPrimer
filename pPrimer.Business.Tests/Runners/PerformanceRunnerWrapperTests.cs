using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using pPrimer.Business.Runners;

namespace pPrimer.Business.Tests.Runners
{
    using Moq;

    [TestClass]
    public class PerformanceRunnerWrapperTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfRunnerIsNull()
        {
            var runner = (IRunner)null;

            var result = new PerformanceRunnerWrapper(runner);
        }

        [TestMethod]
        public void ShouldHaveProperReturnObjectAfterGetAllNumbersExecution()
        {
            var testPrimeResult = new[] { 2, 5, 7 };
            var anyTopNumber = 100;
            var runner = new Mock<IRunner>();
            runner.Setup(x => x.GetAllNumbers(It.IsAny<int>())).Returns(testPrimeResult);
            var performanceRunner = new PerformanceRunnerWrapper(runner.Object);

            performanceRunner.GetAllNumbers(anyTopNumber);

            Assert.AreNotEqual(default(DateTime), performanceRunner.Result.StartTime);
            Assert.AreNotEqual(default(DateTime), performanceRunner.Result.EndTime);
            Assert.AreEqual(testPrimeResult.Length, performanceRunner.Result.Primes.Count());
        }

        [TestMethod]
        public void ShouldCallGetAllNumbersOnWrappedRunner()
        {
            var anyTopNumber = 100;
            var runner = new Mock<IRunner>();
            runner.Setup(x => x.GetAllNumbers(It.IsAny<int>())).Returns(new int[] { });
            var performanceRunner = new PerformanceRunnerWrapper(runner.Object);

            performanceRunner.GetAllNumbers(anyTopNumber);

            runner.Verify(x => x.GetAllNumbers(It.IsAny<int>()), Times.Once);
        }
    }
}
