using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;

namespace pPrimer.Monitor.Loggers
{
    public class LogWrapper: ILogWrapper
    {
        private readonly ILog _logger;
        public LogWrapper(ILog logger)
        {
            _logger = logger;
        }

        public void PostToConcole(string message)
        {
            _logger.Info(message);
        }

        public void PostToStateLog(string message)
        {
            _logger.Debug(message);
        }

        public void PostToLog(string message)
        {
            _logger.Error(message);
        }

    }
}
