using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace pPrimer.Business.Tests
{
    [TestClass]
    public class PrimeMethodSetTests
    {
        [TestMethod]
        public void EachInstanceOfPrimeMethodSetHasUniqueId()
        {
            var anyMethodContainer = new PrimeMethodContainer(string.Empty, string.Empty, typeof(IPrimeChecker));
            var anyRunnerContainer = new PrimeRunnerContainer(string.Empty, string.Empty, typeof(IRunner));

            var pms1 = new PrimeMethodSet(anyMethodContainer, anyRunnerContainer);
            var pms2 = new PrimeMethodSet(anyMethodContainer, anyRunnerContainer);

            Assert.AreEqual(0, pms1.Id);
            Assert.AreEqual(1, pms2.Id);
        }
    }
}
