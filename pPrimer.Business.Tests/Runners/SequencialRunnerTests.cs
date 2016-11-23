using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using pPrimer.Business;

namespace pPrimer.Business.Tests.Runners
{ 
    [TestClass]
    public class SequencialRunnerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfCheckerIsNull()
        {
            var checker = (IPrimeChecker)null;

            var result = new SequencialRunner(checker);
        }

        [TestMethod]
        public void ShouldCalculatePrimeNumbers()
        {
            var checker = new PrimeSquareBasedMethod();
            var runner = new SequencialRunner(checker);
            var topLimit = 100;

            var result = runner.GetAllNumbers(topLimit);

            Assert.AreEqual(25, result.Count());
        }

        [TestMethod]
        public void ShouldCallChecker()
        {
            var checker = new Mock<IPrimeChecker>();
            checker.Setup(x => x.IsPrime(It.IsAny<int>()));
            var runner = new SequencialRunner(checker.Object);
            var topLimit = 100;

            var result = runner.GetAllNumbers(topLimit);

            checker.Verify(x => x.IsPrime(It.IsAny<int>()), Times.AtMost(topLimit - 2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfGetAllNumbersReceivesIncorrectArgument()
        {
            var checker = new Mock<IPrimeChecker>();
            var runner = new SequencialRunner(checker.Object);
            var topLimit = 1;

            runner.GetAllNumbers(topLimit);
        }
    }
}
