using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pPrimer.Business.Services
{
    public interface IPerformanceService
    {
        Task<IEnumerable<PerformanceState>> GetState();
    }
}
