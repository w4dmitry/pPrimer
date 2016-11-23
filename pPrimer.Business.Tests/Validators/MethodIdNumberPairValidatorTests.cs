using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentValidation;

using pPrimer.Business.Validators;

namespace pPrimer.Business.Tests.Validators
{
    [TestClass]
    public class MethodIdNumberPairValidatorTests
    {
        [TestMethod]
        public void ValidMethodIdNumberPairShouldNotHaveValidationErrors()
        {
            var anyMethodSetId = 0;
            var anyTopNumber = int.MaxValue;
            var methodIdNumberPairs = new MethodIdNumberPair(anyMethodSetId, anyTopNumber);
            var validator = new MethodIdNumberPairValidator();
            
            var result = validator.Validate(methodIdNumberPairs);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void InvalidTopNumberFailsValidation()
        {
            var anyMethodSetId = 0;
            var invalidTopNumber = 0;
            var methodIdNumberPairs = new MethodIdNumberPair(anyMethodSetId, invalidTopNumber);
            var validator = new MethodIdNumberPairValidator();

            var result = validator.Validate(methodIdNumberPairs);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void InvalidMethodFailsValidation()
        {
            var invalidMethodSetId = int.MaxValue;
            var anyTopNumber = PrimeNumber.FIRST_PRIME_NUMBER;
            var methodIdNumberPairs = new MethodIdNumberPair(invalidMethodSetId, anyTopNumber);
            var validator = new MethodIdNumberPairValidator();

            var result = validator.Validate(methodIdNumberPairs);

            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void ValidContainerPassesValidation()
        {
            var anyMethodSetIdOne = 0;
            var anyMethodSetIdTwo = 1;
            var anyTopNumber = PrimeNumber.FIRST_PRIME_NUMBER;
            var methodIdNumberPairs = new MethodIdNumberPairContainer
                                          {
                                              MethodIdNumberPairs = new[]
                                                                        {
                                                                            new MethodIdNumberPair(anyMethodSetIdOne, anyTopNumber),
                                                                            new MethodIdNumberPair(anyMethodSetIdTwo, anyTopNumber)
                                                                        }
                                          };
            var validator = new MethodIdNumberPairContainerValidator();

            var result = validator.Validate(methodIdNumberPairs);

            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void EvenOneInvalidPairInContainerFailsValidation()
        {
            var anyMethodSetIdOne = 0;
            var invalidMethodSetIdTwo = int.MaxValue;
            var anyTopNumber = PrimeNumber.FIRST_PRIME_NUMBER;
            var methodIdNumberPairs = new MethodIdNumberPairContainer
            {
                MethodIdNumberPairs = new[]
                                                                        {
                                                                            new MethodIdNumberPair(anyMethodSetIdOne, anyTopNumber),
                                                                            new MethodIdNumberPair(invalidMethodSetIdTwo, anyTopNumber)
                                                                        }
            };
            var validator = new MethodIdNumberPairContainerValidator();

            var result = validator.Validate(methodIdNumberPairs);

            Assert.IsFalse(result.IsValid);
        }
    }
}
