using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using pPrimer.Business.Services;
using pPrimer.Web.Models;

namespace pPrimer.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class PerformanceController : Controller
    {
        private readonly IPerformanceService _performanceService;

        public PerformanceController(IPerformanceService performanceService)
        {
            _performanceService = performanceService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> State()
        {
            DateTime start = DateTime.UtcNow;

            var states = await _performanceService.GetState();
            var model = new PerformanceStateListViewModel(states);

            var res = Json(model.States);

            DateTime end = DateTime.UtcNow;
            Debug.WriteLine($"State in {(end - start).TotalMilliseconds} ms.");

            return res;
        }
    }
}