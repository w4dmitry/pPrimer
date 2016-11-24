using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Ajax.Utilities;

using pPrimer.Business;
using pPrimer.Business.Services;
using pPrimer.Web.Models;

namespace pPrimer.Web.Controllers
{
    using System.Diagnostics;

    public class PrimeController : Controller
    {
        private readonly IPrimeService _primeService;

        private readonly JsonResult _badResult;

        public PrimeController(IPrimeService primeService)
        {
            _primeService = primeService;

            _badResult = Json(new { result = false });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            var model = new PrimeMethodsViewModel(_primeService.MethodsSets.Select(m => new PrimeMethodViewModel { Text = m.DisplayName, Value = m.Id, TopNumber = m.TopNumber }).ToList());

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Calculate(IList<PrimeMethodViewModel> methods)
        {
            DateTime start = DateTime.UtcNow;
            
            if(methods == null)
                return _badResult;

            // Map
            var methodIdPairs = methods.Where(method => method.Selected)
                                       .Select(method => new MethodIdNumberPair(method.Value, method.TopNumber));

            var sessionId = _primeService.StartCalculation(new MethodIdNumberPairContainer {MethodIdNumberPairs = methodIdPairs });

            var result = string.IsNullOrWhiteSpace(sessionId) ? _badResult : Json(new { result = true, sid = sessionId });

            DateTime end = DateTime.UtcNow;
            Debug.WriteLine($"Calculate in {(end-start).TotalMilliseconds} ms.");
            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetStatus(string sid)
        {
            DateTime start = DateTime.UtcNow;

            var result = _primeService.GetStatus(sid);
            var model = new StatusModel(result);

            var res = Json(model);
            
            DateTime end = DateTime.UtcNow;
            Debug.WriteLine($"GetStatus in {(end - start).TotalMilliseconds} ms.");

            return res;
        }
    }
}