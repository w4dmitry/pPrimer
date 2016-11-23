using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace pPrimer.Business.Tests
{
    [TestClass]
    public class PrimeRunnerContainerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfRunnerTypeIsInvalid()
        {
            var anyType = typeof(DateTime);

            var container = new PrimeRunnerContainer(string.Empty, string.Empty, anyType);
        }
    }
}
