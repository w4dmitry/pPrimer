using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pPrimer.Business
{
    public interface IRunner
    {
        IEnumerable<int> GetAllNumbers(int topLimit);
        string DisplayName { get; }
    }
}
