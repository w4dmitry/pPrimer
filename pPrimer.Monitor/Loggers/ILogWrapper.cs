using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pPrimer.Monitor.Loggers
{
    public interface ILogWrapper
    {
        void PostToConcole(string message);

        void PostToStateLog(string message);

        void PostToLog(string message);
    }
}
