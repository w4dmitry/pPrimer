using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace pPrimer.Business.Tests
{
    [TestClass]
    public class PrimeSquareBasedMethodTests
    {
        [TestMethod]
        public void MethodShouldCheckPrimeNumbersCorrectly()
        {
            var primes = new[]
                             {
                                 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79,
                                 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173,
                                 179, 181, 191, 193, 197, 199
                             };

            var method = new PrimeSquareBasedMethod();

            Assert.IsTrue(primes.Select(prime => method.IsPrime(prime)).All(x => x));
        }

        [TestMethod]
        public void MethodShouldReturnFalseForNotPrimeNumbers()
        {
            var primes = new[]
                             {
                                 0, 1, 4, 6, 10, 12, 15, 18, 24, 28, 33, 36, 44, 45, 48, 51, 54, 63, 65, 68, 69, 72
                             };

            var method = new PrimeSquareBasedMethod();

            Assert.IsTrue(primes.Select(prime => method.IsPrime(prime)).All(x => !x));
        }
    }
}
