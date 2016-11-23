using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace pPrimer.Business.Tests
{
    [TestClass]
    public class PrimeMethodContainerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfMethodTypeIsInvalid()
        {
            var anyType = typeof(DateTime);

            var container = new PrimeMethodContainer(string.Empty, string.Empty, anyType);
        }
    }
}
