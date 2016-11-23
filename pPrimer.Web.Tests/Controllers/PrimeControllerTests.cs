using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pPrimer.Web;
using pPrimer.Web.Controllers;

using Moq;

using pPrimer.Business.Services;
using System.Collections;
using System.Web;
using System.Web.Script.Serialization;

using pPrimer.Business;
using pPrimer.Web.Models;

namespace pPrimer.Web.Tests.Controllers
{
    [TestClass]
    public class PrimeControllerTests
    {
        [TestMethod]
        public void GivenViewModelActionCalculateShouldProperStatusJsonObject()
        {
            var sid = Guid.NewGuid().ToString("N");
            var controller = GetController(sid, r => { });
            var viewModels = GetViewModels(true, false);

            var result = controller.Calculate(viewModels) as JsonResult;

            var props = GetJsonResultProperties(result);
            Assert.IsNotNull(result);
            Assert.IsTrue(props.Item1);
            Assert.AreEqual(sid, props.Item2);
        }


        [TestMethod]
        public void GivenViewModelWithSomeMethodsSelectedActionCalculateShouldFilterOutNotSelectedMethods()
        {
            IEnumerable<MethodIdNumberPair> filteredResult = null;
            var sid = Guid.NewGuid().ToString("N");
            var controller = GetController(sid, r => filteredResult = r.MethodIdNumberPairs);
            var viewModels = GetViewModels(true, false);

            controller.Calculate(viewModels);

            Assert.IsNotNull(filteredResult);
            Assert.AreEqual(1, filteredResult.Count());
        }

        [TestMethod]
        public void GivenViewModelWithAllMethodsSelectedActionCalculateShouldReceiveAllMethods()
        {
            IEnumerable<MethodIdNumberPair> filteredResult = null;
            var sid = Guid.NewGuid().ToString("N");
            var controller = GetController(sid, r => filteredResult = r.MethodIdNumberPairs);
            var viewModels = GetViewModels(true, true);

            controller.Calculate(viewModels);

            Assert.IsNotNull(filteredResult);
            Assert.AreEqual(2, filteredResult.Count());
        }

        #region Helpers

        private PrimeController GetController(string sid, Action<MethodIdNumberPairContainer> storeResultAction)
        {
            var primeService = new Mock<IPrimeService>();
            primeService.Setup(x => x.StartCalculation(It.IsAny<MethodIdNumberPairContainer>())).Returns(sid).Callback(storeResultAction);
            return new PrimeController(primeService.Object);
        }

        private List<PrimeMethodViewModel> GetViewModels(bool isSelectedOne, bool isselectedTwo)
        {
            var selectedMethod = new PrimeMethodViewModel { Selected = isSelectedOne, Value = 0, TopNumber = int.MaxValue };
            var notSelectedMethod = new PrimeMethodViewModel { Selected = isselectedTwo, Value = 1, TopNumber = int.MaxValue };
            return new List<PrimeMethodViewModel> { notSelectedMethod, selectedMethod };
        }

        public Tuple<bool,string> GetJsonResultProperties(JsonResult jsonResult)
        {
            var data = jsonResult.Data;
            var type = data.GetType();
            
            // result
            var resultPropertyInfo = type.GetProperty("result");
            var resultValue = resultPropertyInfo.GetValue(data, null);

            // sid
            var sidPropertyInfo = type.GetProperty("sid");
            var sidValue = sidPropertyInfo.GetValue(data, null);

            return Tuple.Create((bool)resultValue, (string)sidValue);
        }
        #endregion
    }
}
