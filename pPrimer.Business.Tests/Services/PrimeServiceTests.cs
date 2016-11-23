using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using log4net;

using Moq;

using pPrimer.Business.Services;
using pPrimer.Business.TypeResolver;

using FluentValidation;
using FluentValidation.Results;

using pPrimer.Business;
using pPrimer.Business.Runners;

namespace pPrimer.Business.Tests.Services
{
    using System.Threading;

    using FluentValidation.Results;

    [TestClass]
    public class PrimeServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfResolverIsNull()
        {
            var resolver = (IPrimeFacilitiesResolver)null;
            var logger = new Mock<ILog>();
            var validator = new Mock<IValidator<MethodIdNumberPairContainer>>();

            var result = new PrimeService(resolver, logger.Object, validator.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfLoggerIsNull()
        {
            var resolver = new Mock<IPrimeFacilitiesResolver>();
            var logger = (ILog)null;
            var validator = new Mock<IValidator<MethodIdNumberPairContainer>>();

            var result = new PrimeService(resolver.Object, logger, validator.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfValidatorIsNull()
        {
            var resolver = new Mock<IPrimeFacilitiesResolver>();
            var logger = new Mock<ILog>();
            var validator = (IValidator<MethodIdNumberPairContainer>)null;

            var result = new PrimeService(resolver.Object, logger.Object, validator);
        }

        [TestMethod]
        public void StartCalculationShouldReturnNullForNull()
        {
            var resolver = new Mock<IPrimeFacilitiesResolver>();
            var logger = new Mock<ILog>();
            var validator = new Mock<IValidator<MethodIdNumberPairContainer>>();
            var primeService = new PrimeService(resolver.Object, logger.Object, validator.Object);

            var result = primeService.StartCalculation(null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void StartCalculationShouldReturnNullForEmptyMethodIdNumberPairs()
        {
            var resolver = new Mock<IPrimeFacilitiesResolver>();
            var logger = new Mock<ILog>();
            var methodIdNumberPairs = new MethodIdNumberPairContainer
                                          {
                                              MethodIdNumberPairs = new List<MethodIdNumberPair>()
                                          };
            var validator = new Mock<IValidator<MethodIdNumberPairContainer>>();
            var primeService = new PrimeService(resolver.Object, logger.Object, validator.Object);

            var result = primeService.StartCalculation(methodIdNumberPairs);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void StartCalculationShouldReturnNullIfNoMantchFoundForMethodIdNumberPairs()
        {
            var notExistingMethodId = int.MaxValue;
            var anyTopNumber = int.MaxValue;
            var resolver = new Mock<IPrimeFacilitiesResolver>();
            var logger = new Mock<ILog>();
            var validator = new Mock<IValidator<MethodIdNumberPairContainer>>();
            validator.Setup(x => x.Validate(It.IsAny<MethodIdNumberPairContainer>())).Returns(new ValidationResultTest { IsValidEx = true });
            var methodIdNumberPairs = new MethodIdNumberPairContainer { MethodIdNumberPairs = new List<MethodIdNumberPair> { new MethodIdNumberPair(notExistingMethodId, anyTopNumber) } };

            var primeService = new PrimeService(resolver.Object, logger.Object, validator.Object);

            var result = primeService.StartCalculation(methodIdNumberPairs);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GevenProperMethodIdNumberPairsStartCalculationShouldReturnSid()
        {
            // Arrange
            var anyMethodSetId = 0;
            var anyTopNumber = int.MaxValue;
            var primeService = GetPrimeServiceMock(anyMethodSetId);
            var methodIdNumberPairs = new MethodIdNumberPairContainer { MethodIdNumberPairs = new List<MethodIdNumberPair> { new MethodIdNumberPair(anyMethodSetId, anyTopNumber) }};

            // Act
            var sid = primeService.StartCalculation(methodIdNumberPairs);

            // Assert
            Assert.IsNotNull(sid);
        }

        [TestMethod]
        public void GevenProperSidGetStatusShouldReturnProperStatus()
        {
            var anyMethodSetId = 0;
            var anyTopNumber = int.MaxValue;
            var primeService = GetPrimeServiceMock(anyMethodSetId);
            var methodIdNumberPairs = new MethodIdNumberPairContainer { MethodIdNumberPairs = new List<MethodIdNumberPair> { new MethodIdNumberPair(anyMethodSetId, anyTopNumber) }};

            var sid = primeService.StartCalculation(methodIdNumberPairs);
            var result = primeService.GetStatus(sid);

            Assert.IsInstanceOfType(result, typeof(PrimeCalculationStatus));
        }

        [TestMethod]
        public void GevenInvalidSidGetStatusShouldReturnNull()
        {
            var invalidSid = string.Empty;
            var primeService = GetPrimeServiceMock();

            var result = primeService.GetStatus(invalidSid);

            Assert.IsNull(result);
        }

        #region Helpers

        public class ValidationResultTest : ValidationResult
        {
            public bool IsValidEx { get; set; }

            public override bool IsValid => IsValidEx;
        }

        private PrimeService GetPrimeServiceMock(int anyMethodSetId = int.MinValue)
        {
            var logger = new Mock<ILog>();
            var methodContainer = new Mock<PrimeMethodContainer>(string.Empty, string.Empty, typeof(IPrimeChecker));
            var runnerContainer = new Mock<PrimeRunnerContainer>(string.Empty, string.Empty, typeof(IRunner));

            var primeMethodSet = new Mock<PrimeMethodSet>(methodContainer.Object, runnerContainer.Object);

            if(anyMethodSetId > 0)
                primeMethodSet.SetupGet(x => x.Id).Returns(anyMethodSetId);
            else
                primeMethodSet.SetupGet(x => x.Id);

            var anyRunner = new Mock<IRunner>();
            anyRunner.Setup(x => x.GetAllNumbers(It.IsAny<int>())).Returns(new[] { PrimeNumber.FIRST_PRIME_NUMBER });
            var performanceRunner = new PerformanceRunnerWrapper(anyRunner.Object);

            var resolver = new Mock<IPrimeFacilitiesResolver>();
            resolver.SetupGet(x => x.MethodSets).Returns(new[] { primeMethodSet.Object });
            resolver.Setup(x => x.GetPerformanceRunner(It.IsAny<Type>(), It.IsAny<Type>())).Returns(performanceRunner);

            var validator = new Mock<IValidator<MethodIdNumberPairContainer>>();
            validator.Setup(x => x.Validate(It.IsAny<MethodIdNumberPairContainer>())).Returns(new ValidationResultTest { IsValidEx = true });

            var primeService = new PrimeService(resolver.Object, logger.Object, validator.Object);
            return primeService;
        }
        #endregion
    }
}
